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
#ifdef _MSC_VER
#pragma warning (disable : 4786)
#endif
#include <cstring>
#include <cstdio>
#include <cstdlib>
#include <algorithm>
#include <functional>
#include <memory>
#include <boost/bind.hpp>
#include "winconst.h"
#include "winbit32.h"
#include "dtwainc.h"
#include "ctltwmgr.h"
#include "ctldib.h"
#include "enumeratorfuncs.h"
#include "ctlfileutils.h"
/* Header signatures for various resources */
#define BFT_ICON   0x4349   /* 'IC' */
#define BFT_BITMAP 0x4d42   /* 'BM' */
#define BFT_CURSOR 0x5450   /* 'PT' */
#define MAXREAD     65535
#define BOUND(x,min,max) ((x) < (min) ? (min) : ((x) > (max) ? (max) : (x)))

#define LPBimage(lpbi)  ((HPSTR)lpbi+lpbi->biSize+(long)(lpbi->biClrUsed*sizeof(RGBQUAD)))
#define LPBlinewidth(lpbi) (WIDTHBYTES((WORD)lpbi->biWidth*lpbi->biBitCount))

#ifdef USE_NAMESPACES
using namespace std;
#endif
using namespace dynarithmic;

class FindHandlePred : unary_function<CTL_TwainDibPtr, bool>
{
    public:
        FindHandlePred(HANDLE h) :
          m_handle(h) {}
        bool operator () (CTL_TwainDibPtr Dib) const
        {return (Dib->GetHandle() == m_handle); }

    private:
        HANDLE m_handle;
};

CTL_TwainDibInfo::CTL_TwainDibInfo() : m_hDib(nullptr), m_hPal(nullptr)
{}

bool CTL_TwainDibInfo::operator == (const CTL_TwainDibInfo& rInfo) const
{
    if ( this == &rInfo )
        return true;
    return (m_hPal == rInfo.m_hPal &&
            m_hDib == rInfo.m_hDib
        );
}

void CTL_TwainDibInfo::SetDib(HANDLE hDib)
{
    m_hDib = hDib;
    #if 0
    DWORD gSize = 0;
    #ifdef _WIN32
        BYTE *gl = (BYTE *)GlobalLock(hDib);
        gSize = GlobalSize(hDib);
        m_fipMemoryIO = std::make_shared<fipMemoryIO>(gl , gSize);
        m_fipImage = std::make_shared<fipImage>();
        m_fipImage->loadFromMemory(FIF_DIB, *m_fipMemoryIO.get(), gSize);
    #else
        BYTE *gl = (BYTE *)hDib;
        gSize = 100000000;
        m_fipMemoryIO = std::make_shared<fipMemoryIO>(gl , gSize);
        m_fipImage = std::make_shared<fipImage>();
        m_fipImage->loadFromMemory(FIF_TIFF, *m_fipMemoryIO->get(), 0);
    #endif
    #endif
}

void CTL_TwainDibInfo::SetPalette( HPALETTE hPal )
{ m_hPal = hPal; }

void CTL_TwainDibInfo::DeleteAllDibInfo()
{
    DeleteDibPalette();
    DeleteDib();
}

void CTL_TwainDibInfo::DeleteDibPalette()
{
    if (m_hPal)
    {
        ImageMemoryHandler::DeleteObject((HGDIOBJ)m_hPal);
        m_hPal = NULL;
    }
}

void CTL_TwainDibInfo::DeleteDib()
{
    if (m_hDib)
    {
        ImageMemoryHandler::GlobalFree(m_hDib);
        m_hDib = NULL;
    }
}

HANDLE CTL_TwainDibInfo::GetDib() const
{
    return m_hDib;
}

HPALETTE CTL_TwainDibInfo::GetPalette() const
{
    return m_hPal;
}

/////////////////////////////////////////////////////////////////////////
// DIB utilities (Dib utilities classed from the EZ_TWain application  //
/////////////////////////////////////////////////////////////////////////

//---------------------------------------------------------------------------
/*PALETTEENTRY CTL_TwainDib::s_peStock256[] =
{

    {     0,   0,   0, 0},
    {   128,   0,   0, 0},    // 001 dark red
    {     0, 128,   0, 0},    // 002 dark green
    {   128, 128,   0, 0},    // 003 dark brown
    {     0,   0, 128, 0},    // 004 dark blue
    {   128,   0, 128, 0},    // 005 dark purple
    {     0, 128, 128, 0},    // 006 dark teal
    {   192, 192, 192, 0},    // 007 light gray
    {   192, 220, 192, 0},    // 008 pale green
    {   166, 202, 240, 0},    // 009 sky blue
    {     4,   4,   4, 4},    // 010 dark gray ramp
    {     8,   8,   8, 4},    // 011
    {    12,  12,  12, 4},    // 012
    {    17,  17,  17, 4},    // 013
    {    22,  22,  22, 4},    // 014
    {    28,  28,  28, 4},    // 015
    {    34,  34,  34, 4},    // 016
    {    41,  41,  41, 4},    // 017
    {    85,  85,  85, 4},    // 018
    {    77,  77,  77, 4},    // 019
    {    66,  66,  66, 4},    // 020
    {    57,  57,  57, 4},    // 021
    {   255, 124, 128, 4},    // 022
    {   255,  80,  80, 4},    // 023
    {   214,   0, 147, 4},    // 024
    {   204, 236, 255, 4},    // 025
    {   239, 214, 198, 4},    // 026
    {   231, 231, 214, 4},    // 027
    {   173, 169, 144, 4},    // 028
    {    51,   0,   0, 4},    // 029
    {   102,   0,   0, 4},    // 030
    {   153,   0,   0, 4},    // 031
    {   204,   0,   0, 4},    // 032
    {     0,  51,   0, 4},    // 033
    {    51,  51,   0, 4},    // 034
    {   102,  51,   0, 4},    // 035
    {   153,  51,   0, 4},    // 036
    {   204,  51,   0, 4},    // 037
    {   255,  51,   0, 4},    // 038
    {     0, 102,   0, 4},    // 039
    {    51, 102,   0, 4},    // 040
    {   102, 102,   0, 4},    // 041
    {   153, 102,   0, 4},    // 042
    {   204, 102,   0, 4},    // 043
    {   255, 102,   0, 4},    // 044
    {     0, 153,   0, 4},    // 045
    {    51, 153,   0, 4},    // 046
    {   102, 153,   0, 4},    // 047
    {   153, 153,   0, 4},    // 048
    {   204, 153,   0, 4},    // 049
    {   255, 153,   0, 4},    // 050
    {     0, 204,   0, 4},    // 051
    {    51, 204,   0, 4},    // 052
    {   102, 204,   0, 4},    // 053
    {   153, 204,   0, 4},    // 054
    {   204, 204,   0, 4},    // 055
    {   255, 204,   0, 4},    // 056
    {   102, 255,   0, 4},    // 057
    {   153, 255,   0, 4},    // 058
    {   204, 255,   0, 4},    // 059
    {     0,   0,  51, 4},    // 060
    {    51,   0,  51, 4},    // 061
    {   102,   0,  51, 4},    // 062
    {   153,   0,  51, 4},    // 063
    {   204,   0,  51, 4},    // 064
    {   255,   0,  51, 4},    // 065
    {     0,  51,  51, 4},    // 066
    {    51,  51,  51, 4},    // 067
    {   102,  51,  51, 4},    // 068
    {   153,  51,  51, 4},    // 069
    {   204,  51,  51, 4},    // 070
    {   255,  51,  51, 4},    // 071
    {     0, 102,  51, 4},    // 072
    {    51, 102,  51, 4},    // 073
    {   102, 102,  51, 4},    // 074
    {   153, 102,  51, 4},    // 075
    {   204, 102,  51, 4},    // 076
    {   255, 102,  51, 4},    // 077
    {     0, 153,  51, 4},    // 078
    {    51, 153,  51, 4},    // 079
    {   102, 153,  51, 4},    // 080
    {   153, 153,  51, 4},    // 081
    {   204, 153,  51, 4},    // 082
    {   255, 153,  51, 4},    // 083
    {     0, 204,  51, 4},    // 084
    {    51, 204,  51, 4},    // 085
    {   102, 204,  51, 4},    // 086
    {   153, 204,  51, 4},    // 087
    {   204, 204,  51, 4},    // 088
    {   255, 204,  51, 4},    // 089
    {    51, 255,  51, 4},    // 090
    {   102, 255,  51, 4},    // 091
    {   153, 255,  51, 4},    // 092
    {   204, 255,  51, 4},    // 093
    {   255, 255,  51, 4},    // 094
    {     0,   0, 102, 4},    // 095
    {    51,   0, 102, 4},    // 096
    {   102,   0, 102, 4},    // 097
    {   153,   0, 102, 4},    // 098
    {   204,   0, 102, 4},    // 099
    {   255,   0, 102, 4},    // 100
    {     0,  51, 102, 4},    // 101
    {    51,  51, 102, 4},    // 102
    {   102,  51, 102, 4},    // 103
    {   153,  51, 102, 4},    // 104
    {   204,  51, 102, 4},    // 105
    {   255,  51, 102, 4},    // 106
    {     0, 102, 102, 4},    // 107
    {    51, 102, 102, 4},    // 108
    {   102, 102, 102, 4},    // 109
    {   153, 102, 102, 4},    // 110
    {   204, 102, 102, 4},    // 111
    {     0, 153, 102, 4},    // 112
    {    51, 153, 102, 4},    // 113
    {   102, 153, 102, 4},    // 114
    {   153, 153, 102, 4},    // 115
    {   204, 153, 102, 4},    // 116
    {   255, 153, 102, 4},    // 117
    {     0, 204, 102, 4},    // 118
    {    51, 204, 102, 4},    // 119
    {   153, 204, 102, 4},    // 120
    {   204, 204, 102, 4},    // 121
    {   255, 204, 102, 4},    // 122
    {     0, 255, 102, 4},    // 123
    {    51, 255, 102, 4},    // 124
    {   153, 255, 102, 4},    // 125
    {   204, 255, 102, 4},    // 126
    {   255,   0, 204, 4},    // 127
    {   204,   0, 255, 4},    // 128
    {     0, 153, 153, 4},    // 129
    {   153,  51, 153, 4},    // 130
    {   153,   0, 153, 4},    // 131
    {   204,   0, 153, 4},    // 132
    {     0,   0, 153, 4},    // 133
    {    51,  51, 153, 4},    // 134
    {   102,   0, 153, 4},    // 135
    {   204,  51, 153, 4},    // 136
    {   255,   0, 153, 4},    // 137
    {     0, 102, 153, 4},    // 138
    {    51, 102, 153, 4},    // 139
    {   102,  51, 153, 4},    // 140
    {   153, 102, 153, 4},    // 141
    {   204, 102, 153, 4},    // 142
    {   255,  51, 153, 4},    // 143
    {    51, 153, 153, 4},    // 144
    {   102, 153, 153, 4},    // 145
    {   153, 153, 153, 4},    // 146
    {   204, 153, 153, 4},    // 147
    {   255, 153, 153, 4},    // 148
    {     0, 204, 153, 4},    // 149
    {    51, 204, 153, 4},    // 150
    {   102, 204, 102, 4},    // 151
    {   153, 204, 153, 4},    // 152
    {   204, 204, 153, 4},    // 153
    {   255, 204, 153, 4},    // 154
    {     0, 255, 153, 4},    // 155
    {    51, 255, 153, 4},    // 156
    {   102, 204, 153, 4},    // 157
    {   153, 255, 153, 4},    // 158
    {   204, 255, 153, 4},    // 159
    {   255, 255, 153, 4},    // 160
    {     0,   0, 204, 4},    // 161
    {    51,   0, 153, 4},    // 162
    {   102,   0, 204, 4},    // 163
    {   153,   0, 204, 4},    // 164
    {   204,   0, 204, 4},    // 165
    {     0,  51, 153, 4},    // 166
    {    51,  51, 204, 4},    // 167
    {   102,  51, 204, 4},    // 168
    {   153,  51, 204, 4},    // 169
    {   204,  51, 204, 4},    // 170
    {   255,  51, 204, 4},    // 171
    {     0, 102, 204, 4},    // 172
    {    51, 102, 204, 4},    // 173
    {   102, 102, 153, 4},    // 174
    {   153, 102, 204, 4},    // 175
    {   204, 102, 204, 4},    // 176
    {   255, 102, 153, 4},    // 177
    {     0, 153, 204, 4},    // 178
    {    51, 153, 204, 4},    // 179
    {   102, 153, 204, 4},    // 180
    {   153, 153, 204, 4},    // 181
    {   204, 153, 204, 4},    // 182
    {   255, 153, 204, 4},    // 183
    {     0, 204, 204, 4},    // 184
    {    51, 204, 204, 4},    // 185
    {   102, 204, 204, 4},    // 186
    {   153, 204, 204, 4},    // 187
    {   204, 204, 204, 4},    // 188
    {   255, 204, 204, 4},    // 189
    {     0, 255, 204, 4},    // 190
    {    51, 255, 204, 4},    // 191
    {   102, 255, 153, 4},    // 192
    {   153, 255, 204, 4},    // 193
    {   204, 255, 204, 4},    // 194
    {   255, 255, 204, 4},    // 195
    {    51,   0, 204, 4},    // 196
    {   102,   0, 255, 4},    // 197
    {   153,   0, 255, 4},    // 198
    {     0,  51, 204, 4},    // 199
    {    51,  51, 255, 4},    // 200
    {   102,  51, 255, 4},    // 201
    {   153,  51, 255, 4},    // 202
    {   204,  51, 255, 4},    // 203
    {   255,  51, 255, 4},    // 204
    {     0, 102, 255, 4},    // 205
    {    51, 102, 255, 4},    // 206
    {   102, 102, 204, 4},    // 207
    {   153, 102, 255, 4},    // 208
    {   204, 102, 255, 4},    // 209
    {   255, 102, 204, 4},    // 210
    {     0, 153, 255, 4},    // 211
    {    51, 153, 255, 4},    // 212
    {   102, 153, 255, 4},    // 213
    {   153, 153, 255, 4},    // 214
    {   204, 153, 255, 4},    // 215
    {   255, 153, 255, 4},    // 216
    {     0, 204, 255, 4},    // 217
    {    51, 204, 255, 4},    // 218
    {   102, 204, 255, 4},    // 219
    {   153, 204, 255, 4},    // 220
    {   204, 204, 255, 4},    // 221
    {   255, 204, 255, 4},    // 222
    {    51, 255, 255, 4},    // 223
    {   102, 255, 204, 4},    // 224
    {   153, 255, 255, 4},    // 225
    {   204, 255, 255, 4},    // 226
    {   255, 102, 102, 4},    // 227
    {   102, 255, 102, 4},    // 228
    {   255, 255, 102, 4},    // 229
    {   102, 102, 255, 4},    // 230
    {   255, 102, 255, 4},    // 231
    {   102, 255, 255, 4},    // 232
    {   165,   0,  33, 4},    // 233
    {    95,  95,  95, 4},    // 234
    {   119, 119, 119, 4},    // 235
    {   134, 134, 134, 4},    // 236
    {   150, 150, 150, 4},    // 237
    {   203, 203, 203, 4},    // 238
    {   178, 178, 178, 4},    // 239
    {   215, 215, 215, 4},    // 240
    {   221, 221, 221, 4},    // 241
    {   227, 227, 227, 4},    // 242
    {   234, 234, 234, 4},    // 243
    {   241, 241, 241, 4},    // 244
    {   248, 248, 248, 4},    // 245
    {   255, 251, 240, 0},    // 246
    {   160, 160, 164, 0},    // 247
    {   128, 128, 128, 0},    // 248
    {   255,   0,   0, 0},    // 249
    {     0, 255,   0, 0},    // 250
    {   255, 255,   0, 0},    // 251
    {     0,   0, 255, 0},    // 252
    {   255,   0, 255, 0},    // 253
    {     0, 255, 255, 0},    // 254
    {   255, 255, 255, 0}     // 255 always white

};
*/
// Construction
CTL_TwainDib::CTL_TwainDib() : m_bAutoDelete(false), m_bIsValid(false), m_nJpegQuality(75)
{ }

CTL_TwainDib::CTL_TwainDib(HANDLE hDib, HWND hWnd) : m_bAutoDelete(true), m_bIsValid(true)
{
    m_TwainDibInfo.SetDib(hDib);
    // compute or guess a palette to use for display
    // m_TwainDibInfo.SetPalette( CreateDibPalette() );
}

// Read a Dib from a file
CTL_TwainDib::CTL_TwainDib(LPCTSTR lpszFileName, HWND hWnd)
{
    Init();
    m_TwainDibInfo.SetDib( ReadDibBitmap( lpszFileName ));
/*    if ( m_TwainDibInfo.GetDib() )
        m_TwainDibInfo.SetPalette( CreateDibPalette() );*/
}

CTL_TwainDib::CTL_TwainDib( const CTL_TwainDib &rDib )
{ SetEqual( rDib ); }

void CTL_TwainDib::swap(CTL_TwainDib& left, CTL_TwainDib& rt)
{
    std::swap(left.m_TwainDibInfo, rt.m_TwainDibInfo);
    std::swap(left.m_bIsValid, rt.m_bIsValid);
    std::swap(left.m_bJpegProgressive, rt.m_bJpegProgressive);
    std::swap(left.m_nJpegQuality, rt.m_nJpegQuality);
    std::swap(left.m_bAutoDelete, rt.m_bAutoDelete);
    std::swap(left.m_bAutoDeletePalette, rt.m_bAutoDeletePalette);
}

CTL_TwainDib& CTL_TwainDib::operator=(const CTL_TwainDib& rDib)
{
    if ( this != &rDib )
    {
        CTL_TwainDib temp(*this);
        swap(*this, temp);
    }
    return *this;
}

void CTL_TwainDib::SetEqual( const CTL_TwainDib &rDib )
{
    m_bAutoDelete = rDib.m_bAutoDelete;
    CTL_TwainDib *pDib = (CTL_TwainDib *)&rDib;
    pDib->m_bAutoDelete = false;
    m_bIsValid = rDib.m_bIsValid;
    m_TwainDibInfo = rDib.m_TwainDibInfo;
    m_nJpegQuality = rDib.m_nJpegQuality;
    m_bAutoDeletePalette = rDib.m_bAutoDeletePalette;
}

/***************************************************************************
*  PURPOSE    : Will read a file in DIB format and return a global HANDLE  *
*               to it's BITMAPINFO.  This function will work with both     *
*               "old" (BITMAPCOREHEADER) and "new" (BITMAPINFOHEADER)      *
*               bitmap formats, but will always return a "new" BITMAPINFO  *
*                                                                          *
*  RETURNS    : A handle to the BITMAPINFO of the DIB in the file.         *
*                                                                          *
****************************************************************************/
HANDLE CTL_TwainDib::ReadDibBitmap(LPCTSTR)
{
    return NULL;
}


void CTL_TwainDib::SetJpegValues(int nQuality, bool bProgressive)
{
    m_nJpegQuality = nQuality;
    m_bJpegProgressive = bProgressive;
}


int CTL_TwainDib::WriteDibBitmap (DTWAINImageInfoEx& ImageInfo,
                                  LPCTSTR szFile, int nFormat/*=BmpFormat*/,
                                  bool bOpenFile/*=TRUE*/, int fhFile/*=0*/)
{
    std::unique_ptr<CTL_ImageIOHandler> pHandler;
    ImageInfo.IsPDF = false;
    ResolvePostscriptOptions(ImageInfo, nFormat);
    switch (nFormat )
    {
        case BmpFormat:
            pHandler = std::make_unique<CTL_BmpIOHandler>( this );
        break;

        case JpegFormat:
            pHandler = std::make_unique<CTL_JpegIOHandler>( this, ImageInfo);
        break;
        case Jpeg2000Format:
            ImageInfo.nJpegQuality = 100;
            ImageInfo.bProgressiveJpeg = 0;
            pHandler = std::make_unique<CTL_Jpeg2KIOHandler>( this, ImageInfo );
        break;

        case PDFFormat:
        case PDFFormatMULTI:
        {
            ImageInfo.nJpegQuality = 100;
            ImageInfo.bProgressiveJpeg = 0;
            ImageInfo.IsPDF = true;
            pHandler = std::make_unique<CTL_PDFIOHandler>(this, nFormat, ImageInfo);
        }
        break;

        case PngFormat:
            pHandler = std::make_unique<CTL_PngIOHandler>( this, ImageInfo );
        break;

        case PcxFormat:
            pHandler = std::make_unique<CTL_PcxIOHandler>( this, nFormat, ImageInfo );
        break;
        case TiffFormatNONE:
        case TiffFormatGROUP4:
        case TiffFormatGROUP3:
        case TiffFormatLZW:
        case TiffFormatPACKBITS:
        case TiffFormatDEFLATE:
        case TiffFormatJPEG:
        case TiffFormatPIXARLOG:
        case PSFormatLevel1:
        case PSFormatLevel2:
        case PSFormatLevel3:
            if ( nFormat == PSFormatLevel1 || nFormat == PSFormatLevel2 || nFormat == PSFormatLevel3 )
                ImageInfo.IsPostscript = TRUE;
            pHandler = std::make_unique<CTL_TiffIOHandler>( this, nFormat, ImageInfo );
        break;
        case TgaFormat:
            pHandler = std::make_unique<CTL_TgaIOHandler>( this );
        break;

        case WmfFormat:
        case EmfFormat:
            pHandler = std::make_unique<CTL_WmfIOHandler>( this, nFormat );
        break;

        case PsdFormat:
            pHandler = std::make_unique<CTL_PsdIOHandler>( this );
        break;

        case GifFormat:
            pHandler = std::make_unique<CTL_GifIOHandler>( this );
        break;

        case IcoFormat:
        case IcoVistaFormat:
            ImageInfo.IsVistaIcon = (nFormat == IcoVistaFormat);
            pHandler = std::make_unique<CTL_IcoIOHandler>( this, ImageInfo );
        break;

        case WBMPFormat:
            pHandler = std::make_unique<CTL_WBMPIOHandler>(this, ImageInfo);
        break;

        case WEBPFormat:
            pHandler = std::make_unique<CTL_WebpIOHandler>(this);
        break;

        case PBMFormat:
            pHandler = std::make_unique<CTL_PBMIOHandler>(this);
        break;

        case TextFormat:
        case TextFormatMulti:
        {
            // Get the current OCR engine's input format
            DTWAIN_ARRAY a = 0;
            DTWAINArrayLL_RAII raii(a);
            CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
            DTWAIN_GetOCRCapValues((DTWAIN_OCRENGINE)pHandle->m_pOCRDefaultEngine.get(), DTWAIN_OCRCV_IMAGEFILEFORMAT,
                                    DTWAIN_CAPGETCURRENT, &a);
            auto& vValues = EnumeratorVector<int>(a);
            if ( !vValues.empty() )
            {
                LONG InputFormat = vValues[0];
                pHandler = std::make_unique<CTL_TextIOHandler>(this, InputFormat, ImageInfo, pHandle->m_pOCRDefaultEngine.get());
            }
        }
        break;

        default:
            return DTWAIN_ERR_BADPARAM;
    }
    int bRet;
    try
    {
        bRet = pHandler->WriteBitmap( szFile, bOpenFile, fhFile )?1:0;
    }
    catch(...)
    {
        //Exception error
        bRet = DTWAIN_ERR_EXCEPTION_ERROR;
    }
    if ( bRet != 0)
    {
        // If this error is > 0, this is an ISource error, else this
        // is a DTWAIN error
        if ( bRet > 0 )
            bRet = DTWAIN_ERR_FILEXFERSTART - bRet;

    }
    return bRet;
}

CTL_ImageIOHandlerPtr CTL_TwainDib::WriteFirstPageDibMulti(DTWAINImageInfoEx& ImageInfo, LPCTSTR szFile, int nFormat,
                                                          bool bOpenFile, int fhFile, int &nStatus)
{
    CTL_ImageIOHandlerPtr pHandler;
    ImageInfo.IsPDF = false;
    ResolvePostscriptOptions(ImageInfo, nFormat);

    switch (nFormat )
    {
        case TiffFormatNONEMULTI:
        case TiffFormatGROUP3MULTI:
        case TiffFormatGROUP4MULTI:
        case TiffFormatPACKBITSMULTI:
        case TiffFormatDEFLATEMULTI:
        case TiffFormatJPEGMULTI:
        case TiffFormatLZWMULTI:
        case PSFormatLevel1Multi:
        case PSFormatLevel2Multi:
        case PSFormatLevel3Multi:
        case TiffFormatPIXARLOGMULTI:
            pHandler = std::make_shared<CTL_TiffIOHandler>( this, nFormat, ImageInfo );
        break;

        case DcxFormat:
            pHandler = std::make_shared<CTL_PcxIOHandler>( this, nFormat, ImageInfo );
        break;
        case PDFFormatMULTI:
            ImageInfo.nJpegQuality = 100;
            ImageInfo.bProgressiveJpeg = 0;
            ImageInfo.IsPDF = true;
            pHandler = std::make_shared<CTL_PDFIOHandler>( this, nFormat, ImageInfo);
        break;
        case TextFormatMulti:
        {
            // Get the current OCR engine's input format
            DTWAIN_ARRAY a = 0;
            DTWAINArrayLL_RAII raii(a);
            CTL_TwainDLLHandle *pHandle = static_cast<CTL_TwainDLLHandle *>(GetDTWAINHandle_Internal());
            DTWAIN_GetOCRCapValues((DTWAIN_OCRENGINE)pHandle->m_pOCRDefaultEngine.get(), DTWAIN_OCRCV_IMAGEFILEFORMAT,
                                    DTWAIN_CAPGETCURRENT, &a);
            auto& vValues = EnumeratorVector<int>(a);
            if ( !vValues.empty() )
            {
                LONG InputFormat = vValues[0];
                pHandler = std::make_shared<CTL_TextIOHandler>(this, InputFormat, ImageInfo, pHandle->m_pOCRDefaultEngine.get());
            }
        }
        break;
    }
    if ( !pHandler )
    {
        nStatus = DTWAIN_ERR_BADPARAM;
        return NULL;
    }

    DibMultiPageStruct s;
    s.Stage = DIB_MULTI_FIRST;
    s.strName = szFile;
    s.pUserData = pHandler->GetMultiDibData();
    pHandler->SetMultiDibInfo(s);

    try
    {
        nStatus = pHandler->WriteBitmap( szFile, bOpenFile, fhFile, reinterpret_cast<LONG64>(&s) );
    }
    catch(...)
    {
        // An exception error occurred.
        nStatus = DTWAIN_ERR_EXCEPTION_ERROR;
    }
    if ( nStatus != 0)
    {
        // If this error is > 0, this is an ISource error, else this
        // is a DTWAIN error
        if ( nStatus > 0 )
            nStatus = DTWAIN_ERR_FILEXFERSTART - nStatus;

    }
    pHandler->SetMultiDibData(s.pUserData);
    return pHandler;
}


int CTL_TwainDib::WriteNextPageDibMulti(CTL_ImageIOHandlerPtr& pImgHandler, int &nStatus,
                                        const DTWAINImageInfoEx& ImageInfo)
{
    nStatus = DTWAIN_ERR_BADPARAM;
    if (pImgHandler)
    {
        DibMultiPageStruct s2 = pImgHandler->GetMultiDibInfo();

        s2.Stage = DIB_MULTI_NEXT;
        s2.pUserData = pImgHandler->GetMultiDibData();
        pImgHandler->SetDib(this);
        pImgHandler->SetImageInfo(ImageInfo);
        try
        {
            nStatus = pImgHandler->WriteBitmap(s2.strName.c_str(), false, 0, reinterpret_cast<LONG64>(&s2));
        }
        catch(...)
        {
            // An exception occurred
            nStatus = DTWAIN_ERR_EXCEPTION_ERROR;
        }
        if ( nStatus != 0)
        {
            // If this error is > 0, this is an ISource error, else this
            // is a DTWAIN error
            if ( nStatus > 0 )
                nStatus = DTWAIN_ERR_FILEXFERSTART - nStatus;

        }
        pImgHandler->SetMultiDibData(s2.pUserData);
    }
    return nStatus;
}


int CTL_TwainDib::WriteLastPageDibMulti(CTL_ImageIOHandlerPtr& pImgHandler, int &nStatus, bool bSaveFile/*=true*/)
{
    nStatus = DTWAIN_ERR_BADPARAM;
    if (pImgHandler)
    {
        DibMultiPageStruct s = pImgHandler->GetMultiDibInfo();
        s.Stage = DIB_MULTI_LAST;
        s.pUserData = pImgHandler->GetMultiDibData();

        try
        {
            nStatus = pImgHandler->WriteBitmap(s.strName.c_str(), false, 0, reinterpret_cast<LONG64>(&s));
        }
        catch(...)
        {
            // An exception occurred
            nStatus = DTWAIN_ERR_EXCEPTION_ERROR;
        }
        if ( nStatus != 0)
        {
            // If this error is > 0, this is an ISource error, else this
            // is a DTWAIN error
            if ( nStatus > 0 )
                nStatus = DTWAIN_ERR_FILEXFERSTART - nStatus;

        }

        if ( !bSaveFile)
        {
            // remove the file
            delete_file(s.strName.c_str());
        }
    }
    else
    // null was passed
        nStatus = 0;
    return nStatus;
}

void CTL_TwainDib::ResolvePostscriptOptions(const DTWAINImageInfoEx& Info, int &nFormat )
{
    if ( !Info.IsPostscript )
        return;

    if ( Info.IsPostscriptMultipage )
        nFormat = TiffFormatPACKBITSMULTI;
    else
        nFormat = TiffFormatPACKBITS;
}



/****************************************************************************
 *                                                                          *
 *  FUNCTION   :  PaletteSize(void * pv)                                *
 *                                                                          *
 *  PURPOSE    :  Calculates the palette size in bytes. If the info. block  *
 *                is of the BITMAPCOREHEADER type, the number of colors is  *
 *                multiplied by 3 to give the palette size, otherwise the   *
 *                number of colors is multiplied by 4.                                                          *
 *                                                                          *
 *  RETURNS    :  Palette size in number of bytes.                          *
 *                                                                          *
 ****************************************************************************/
WORD CTL_TwainDib::PaletteSize (void  *pv)
{
    LPBITMAPINFOHEADER lpbi;
    WORD               NumColors;

    lpbi      = (LPBITMAPINFOHEADER)pv;
    NumColors = (WORD)DibNumColors(lpbi);

    if (lpbi->biSize == sizeof(BITMAPCOREHEADER))
        return NumColors * sizeof(RGBTRIPLE);
        return NumColors * sizeof(RGBQUAD);
}

void CTL_TwainDib::Init()
{
    m_bAutoDelete = true;
}

CTL_TwainDib::~CTL_TwainDib()
{
/*    if ( m_bAutoDeletePalette )
        m_TwainDibInfo.DeleteDibPalette();*/
    if ( m_bAutoDelete )
        m_TwainDibInfo.DeleteDib();
}

void CTL_TwainDib::Delete()
{
    m_TwainDibInfo.DeleteAllDibInfo();
}


void CTL_TwainDib::SetHandle(HANDLE hDib, bool bSetPalette/*=TRUE*/)
{
    m_TwainDibInfo.SetDib( hDib );
/*    if ( bSetPalette )
    {
        if ( m_TwainDibInfo.GetPalette() )
            m_TwainDibInfo.DeleteDibPalette();
        m_TwainDibInfo.SetPalette(CreateDibPalette());
    }*/
    m_bIsValid = true;
}

HANDLE CTL_TwainDib::GetHandle() const
{
    return m_TwainDibInfo.GetDib();
}

int CTL_TwainDib::GetDepth() const
{
    HANDLE hDib = m_TwainDibInfo.GetDib();
    if ( !hDib )
        return -1;
    DTWAINGlobalHandle_RAII handler(hDib);
    LPBITMAPINFOHEADER pbi = (LPBITMAPINFOHEADER)ImageMemoryHandler::GlobalLock(hDib);
    int nDepth = pbi->biBitCount;
    return nDepth;
}

int CTL_TwainDib::GetBitsPerPixel() const
{
    HANDLE hDib = m_TwainDibInfo.GetDib();
    if (!hDib)
        return 0;
    DTWAINGlobalHandle_RAII handler(hDib);
    LPBITMAPINFOHEADER pbi = (LPBITMAPINFOHEADER)ImageMemoryHandler::GlobalLock(hDib);

    if (pbi->biSize != sizeof(BITMAPINFOHEADER))
        return 0;

    return pbi->biBitCount;
}

int CTL_TwainDib::GetWidth() const
{
    HANDLE hDib = m_TwainDibInfo.GetDib();
    if ( !hDib )
        return -1;
    DTWAINGlobalHandle_RAII handler(hDib);
   LPBITMAPINFOHEADER pbi = (LPBITMAPINFOHEADER)ImageMemoryHandler::GlobalLock(hDib);
   int nWid = (int)pbi->biWidth;
   return nWid;
}


int CTL_TwainDib::GetHeight() const
{
   HANDLE hDib = m_TwainDibInfo.GetDib();
   DTWAINGlobalHandle_RAII handler(hDib);
   LPBITMAPINFOHEADER pbi = (LPBITMAPINFOHEADER)ImageMemoryHandler::GlobalLock(hDib);
   int nHeight = (int)pbi->biHeight;
   return nHeight;
}

int CTL_TwainDib::GetResolution() const
{
    return ((GetWidth() * GetDepth()) + 7) / 8;
}


int CTL_TwainDib::GetNumColors()  const
{
    HANDLE hDib = m_TwainDibInfo.GetDib();
    if ( !hDib )
        return -1;
    DTWAINGlobalHandle_RAII handler(hDib);
    void  *pv = ImageMemoryHandler::GlobalLock(hDib);
    int nColors = DibNumColors(pv);
    return nColors;
}


int CTL_TwainDib::DibNumColors(void *pv) const
{
    LPBITMAPINFOHEADER    lpbi = ((LPBITMAPINFOHEADER)pv);
    LPBITMAPCOREHEADER    lpbc = ((LPBITMAPCOREHEADER)pv);

    int nColors;

    if (lpbi->biSize == sizeof(BITMAPCOREHEADER))
    {
        nColors = 1 << lpbc->bcBitCount;
    }
    else
    if (lpbi->biClrUsed == 0)
    {
        nColors = 1 << lpbi->biBitCount;
    }
    else
    {
        nColors = (int)lpbi->biClrUsed;
    }
    if (nColors > 256)
        nColors = 0;
    return nColors;
}

#if 0
HPALETTE CTL_TwainDib::CreateDibPalette()
{
    HPALETTE            hPalette = NULL;
    HANDLE hDib = m_TwainDibInfo.GetDib();
    LPBITMAPINFOHEADER  lpbmi = (LPBITMAPINFOHEADER)ImageMemoryHandler::GlobalLock(hDib);

    // make sure we unlock the handle on return using RAII
    DTWAINGlobalHandle_RAII hDibRAII(hDib);

    if (lpbmi)
    {
        WORD nColors = (WORD)GetNumColors();            // size of DIB palette
        WORD nEntries = nColors ? nColors : 256;            // size of palette to create
        UINT nSize = sizeof(LOGPALETTE) + nEntries * sizeof(PALETTEENTRY);

        // allocate logical palette structure
        HGLOBAL hTemp = ImageMemoryHandler::GlobalAlloc(GPTR, nSize);
        LOGPALETTE *pPal = (LOGPALETTE*)ImageMemoryHandler::GlobalLock(hTemp);

        // make sure we clean up using RAII class
        DTWAINGlobalHandleUnlockFree_RAII paletteRAII(hTemp);

        if (pPal)
        {
            // Fill in the palette entries
            pPal->palNumEntries = nEntries;
            pPal->palVersion    = 0x300;            // Windows 3.0 or later
            if (nColors)
            {
                // from the DIB color table
                // Get a pointer to the color table
                RGBQUAD FAR *pRgb = (RGBQUAD FAR *)((LPSTR)lpbmi + (WORD)lpbmi->biSize);
                WORD i;
                // copy from DIB palette (triples, by the way) into the LOGPALETTE
                for (i = 0; i < nEntries; i++)
                {
                    pPal->palPalEntry[i].peRed   = pRgb[i].rgbRed;
                    pPal->palPalEntry[i].peGreen = pRgb[i].rgbGreen;
                    pPal->palPalEntry[i].peBlue  = pRgb[i].rgbBlue;
                    pPal->palPalEntry[i].peFlags = (BYTE)0;
                } // for
            }
            else
            {
                // Deep Dib: Synthesize halftone palette
                memcpy(pPal->palPalEntry, s_peStock256, sizeof(s_peStock256));
            }

            // create a logical palette
            hPalette = CreatePalette(pPal);
        }
    }
    return hPalette;
}
#endif

LPBYTE CTL_TwainDib::DibBits(fipImage& dib)
{
    auto lpdib = dib.getInfoHeader();
    DWORD dwColorTableSize = dib.getPaletteSize(); //PaletteSize( lpdib );
    LPBYTE lpBits = (LPBYTE)lpdib + lpdib->biSize + dwColorTableSize;
    return lpBits;
} // end DibBits



// Only should be called on WM_PAINT of the window
bool CTL_TwainDib::Render()
{
    return true;
}


int CTL_TwainDib::CropDib(FloatRect& ActualRect, FloatRect& RequestedRect,
                          LONG SourceUnit, LONG DestUnit, int dpi, int flags)
{
    HANDLE hDib = m_TwainDibInfo.GetDib();
    if (hDib)
    {
        int retval;
        HANDLE hNewDib = CDibInterface::CropDIB(hDib,
                                            ActualRect,
                                            RequestedRect,
                                            SourceUnit,
                                            DestUnit,
                                            dpi,
                                            flags?true:false,
                                            retval);
        if ( hNewDib )
        {
            m_TwainDibInfo.DeleteDib();
            m_TwainDibInfo.SetDib(hNewDib);
            return 1;
        }
    }
    return 0;
}

bool CTL_TwainDib::IsGrayScale() const
{
    HANDLE hDib = m_TwainDibInfo.GetDib();
    if (hDib)
        return CDibInterface::IsGrayScale(hDib, GetDepth())?true:false;
    return false;
}

bool CTL_TwainDib::IncreaseBpp(unsigned long bpp)
{
    if ( bpp == (unsigned long)GetDepth())
        return true;

    HANDLE hDib = m_TwainDibInfo.GetDib();
    if (hDib)
    {
        HANDLE hNewDib=NULL;

        hNewDib = CDibInterface::IncreaseBpp(hDib, bpp);
        if ( hNewDib )
        {
            m_TwainDibInfo.DeleteDib();
            m_TwainDibInfo.SetDib(hNewDib);
            return true;
        }
    }
    return false;
}

bool CTL_TwainDib::DecreaseBpp(unsigned long bpp)
{
    if ( bpp == (unsigned long)GetDepth())
        return true;

    HANDLE hDib = m_TwainDibInfo.GetDib();
    if (hDib)
    {
        HANDLE hNewDib=NULL;

        hNewDib = CDibInterface::DecreaseBpp(hDib, bpp);
        if ( hNewDib )
        {
            m_TwainDibInfo.DeleteDib();
            m_TwainDibInfo.SetDib(hNewDib);
            return true;
        }
    }
    return false;
}

int CTL_TwainDib::ResampleDib(FloatRect& ResampleRect, int flags)
{
    HANDLE hDib = m_TwainDibInfo.GetDib();
    if (hDib)
    {
        HANDLE hNewDib=NULL;
        if ( flags & CTL_ITwainSource::RESIZE_FLAG)
            hNewDib = CDibInterface::ResampleDIB(hDib, (long)ResampleRect.left, (long)ResampleRect.top);
        if ( hNewDib )
        {
            m_TwainDibInfo.DeleteDib();
            m_TwainDibInfo.SetDib(hNewDib);
            return 1;
        }
    }
    return 0;
}


int CTL_TwainDib::ResampleDib(double xscale, double yscale)
{
    HANDLE hDib = m_TwainDibInfo.GetDib();
    if (hDib)
    {
        HANDLE hNewDib=NULL;
        hNewDib = CDibInterface::ResampleDIB(hDib, xscale, yscale);
        if ( hNewDib )
        {
            m_TwainDibInfo.DeleteDib();
            m_TwainDibInfo.SetDib(hNewDib);
            return 1;
        }
    }
    return 0;
}

int CTL_TwainDib::NegateDib()
{
    HANDLE hDib = m_TwainDibInfo.GetDib();
    if (hDib)
    {
        HANDLE hNewDib=NULL;

        hNewDib = CDibInterface::NegateDIB(hDib);
        if ( hNewDib )
        {
            m_TwainDibInfo.DeleteDib();
            m_TwainDibInfo.SetDib(hNewDib);
            return 1;
        }
    }
    return 0;
}

int CTL_TwainDib::NormalizeDib()
{
    HANDLE hDib = m_TwainDibInfo.GetDib();
    if (hDib)
    {
        HANDLE hNewDib = CDibInterface::NormalizeDib(hDib);
        if ( hNewDib )
        {
            m_TwainDibInfo.DeleteDib();
            m_TwainDibInfo.SetDib(hNewDib);
            return 1;
        }
    }
    return 0;
}

bool CTL_TwainDib::IsBlankDIB(double threshold) const
{
    HANDLE hDib = m_TwainDibInfo.GetDib();
    if (hDib)
        return CDibInterface::IsBlankDIB(hDib, threshold)?true:false;
    return 0;
}

bool CTL_TwainDib::FlipBitMap(bool /*bRGB*/)
{
    HANDLE                  temp;
    LPBITMAPINFO            pdib;
    BYTE*                   pDib;
    HUGEPTR_CHAR            pbuffer;
    HUGEPTR_CHAR/*unsigned char HUGEDEF* */   tempptr;
    HUGEPTR_CHAR/*unsigned char HUGEDEF* */   tempptrsave;
    LONG                    Width;
    LONG                    Height;
    LONG                    Linelength;
    LONG                    indexH;
    DWORD                   SizeImage;
    WORD                    BitCount;
    DWORD                   offset;
    int                     pixels;
    long                    items;
    long                    i;
    BYTE                    SaveRed;
    BYTE                    SaveBlue;

    HANDLE hDib = m_TwainDibInfo.GetDib();

    if (hDib)
    {
        pDib = (BYTE*)ImageMemoryHandler::GlobalLock(hDib);

        DTWAINGlobalHandle_RAII hDibHandler(hDib);

        pdib = (LPBITMAPINFO)pDib;
        Width =     pdib->bmiHeader.biWidth;
        Height =    pdib->bmiHeader.biHeight;
        SizeImage = pdib->bmiHeader.biSizeImage;
        BitCount =  pdib->bmiHeader.biBitCount;

        temp = ImageMemoryHandler::GlobalAlloc(GHND, SizeImage);
        if (temp)
        {
            tempptr = (unsigned char HUGEDEF   *)ImageMemoryHandler::GlobalLock(temp);

            // make sure we unlock and free
            DTWAINGlobalHandleUnlockFree_RAII memHandler(temp);

            tempptrsave = tempptr;

            // calculate offset to start of the bitmap data
            offset = sizeof(BITMAPINFOHEADER);
            offset += pdib->bmiHeader.biClrUsed * sizeof(RGBQUAD);

            Linelength = (((Width*BitCount+31)/32)*4);

            //Goto Last line in bitmap
            offset += (Linelength * (Height-1));
            pDib = pDib + offset - Linelength;

            //For each line
            for (indexH = 1; indexH < Height; indexH++)
            {
                memcpy(tempptr, pDib, Linelength);
                pDib -= (Linelength);
                tempptr += Linelength;
            }

            // Copy temp over hBM
            pbuffer = (unsigned char HUGEDEF *) pdib;
            pbuffer += sizeof(BITMAPINFOHEADER);
            pbuffer += pdib->bmiHeader.biClrUsed * sizeof(RGBQUAD);

            memcpy(pbuffer, tempptrsave, SizeImage);

            //Flip RGB color table
            if ( BitCount == 4 )
            {
                pbuffer = (unsigned char HUGEDEF *) pdib;
                pbuffer += sizeof(BITMAPINFOHEADER);
                pbuffer += pdib->bmiHeader.biClrUsed * sizeof(RGBQUAD);

                pixels = (int)pdib->bmiHeader.biWidth;
                for (items = 0; items < Height; items++)
                {
                    tempptr = pbuffer;
                    for (i=0; i<pixels; i++)
                    {
                        //Switch Red byte and Blue nibble
                        *tempptr = (*tempptr << 4) | (*tempptr >> 4);
                        tempptr++;
                    }
                    pbuffer += Linelength;
                }
            }
            else
            if ( BitCount > 1 && (BitCount != 8)) //bRGB )
            {
                pbuffer = (unsigned char HUGEDEF *) pdib;
                pbuffer += sizeof(BITMAPINFOHEADER);
                pbuffer += pdib->bmiHeader.biClrUsed * sizeof(RGBQUAD);

                pixels = (int)pdib->bmiHeader.biWidth;
                for (items = 0; items < Height; items++)
                {
                    tempptr = pbuffer;
                    for (i=0; i<pixels; i++)
                    {
                        //Switch Red byte and Blue byte
                        SaveRed =  *tempptr;
                        SaveBlue = *(tempptr+2);

                        // Test
                        *tempptr = SaveBlue;;
                        *(tempptr+2) = SaveRed;
                        //increment to next triplet
                        tempptr += 3;
                    }
                    pbuffer += Linelength;
                }
            }
            return true;
        }
    }
    return false;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////
CTL_TwainDibPtr CTL_TwainDibArray::CreateDib()
{
    CTL_TwainDibPtr NewDib(new CTL_TwainDib);
    return InitializeDibInfo(NewDib);
}


CTL_TwainDibPtr CTL_TwainDibArray::CreateDib(HANDLE hDib, HWND hWnd/*=NULL*/)
{
    CTL_TwainDibPtr NewDib(new CTL_TwainDib(hDib, hWnd));
    return InitializeDibInfo(NewDib);
}


CTL_TwainDibPtr CTL_TwainDibArray::CreateDib(LPCTSTR lpszFileName, HWND hWnd/*=NULL*/)
{
    CTL_TwainDibPtr NewDib(new CTL_TwainDib( lpszFileName, hWnd ));
    return InitializeDibInfo(NewDib);
}

CTL_TwainDibPtr CTL_TwainDibArray::CreateDib( const CTL_TwainDib& rDib )
{
    CTL_TwainDibPtr NewDib(new CTL_TwainDib( rDib ));
    return InitializeDibInfo(NewDib);
}

CTL_TwainDibPtr CTL_TwainDibArray::InitializeDibInfo(CTL_TwainDibPtr Dib)
{
    Dib->SetAutoDelete( m_bAutoDelete );
    m_TwainDibArray.push_back(Dib);
    return m_TwainDibArray[m_TwainDibArray.size() - 1];
}

CTL_TwainDibArray::CTL_TwainDibArray(bool bAutoDelete) : m_bAutoDelete(bAutoDelete)
{}

CTL_TwainDibArray::~CTL_TwainDibArray()
{
    RemoveAllDibs();
}


bool CTL_TwainDibArray::RemoveDib( CTL_TwainDibPtr pDib )
{
    auto it = find(m_TwainDibArray.begin(),
              m_TwainDibArray.end(),
              pDib);
    if ( it != m_TwainDibArray.end() )
    {
        m_TwainDibArray.erase(it);
        return true;
    }
    return false;
}

bool CTL_TwainDibArray::RemoveDib( size_t nWhere )
{
    size_t nSize = m_TwainDibArray.size();
    if ( nWhere >= nSize )
        return false;
    m_TwainDibArray.erase(m_TwainDibArray.begin() + nWhere);
    return true;
}


bool CTL_TwainDibArray::RemoveDib( HANDLE hDib )
{
    auto it = find_if(m_TwainDibArray.begin(),
        m_TwainDibArray.end(),
        FindHandlePred(hDib));

    if (it != m_TwainDibArray.end())
    {
        m_TwainDibArray.erase(it);
        return true;
    }
    return false;
}

CTL_TwainDibPtr CTL_TwainDibArray::GetAt(size_t nPos)
{
    #ifdef NO_STL_AT_DEFINED
    return m_TwainDibArray[nPos];
    #else
    return m_TwainDibArray.at(nPos);
    #endif
}

CTL_TwainDibPtr CTL_TwainDibArray::operator[](size_t nPos)
{
    return GetAt(nPos);
}

bool CTL_TwainDibArray::IsAutoDelete() const
{
    return m_bAutoDelete;
}

void CTL_TwainDibArray::RemoveAllDibs()
{
    m_TwainDibArray.clear();
}

bool CTL_TwainDibArray::DeleteDibMemory(CTL_TwainDibPtr Dib)
{
    vector<CTL_TwainDibPtr>::iterator it;
    it = find(m_TwainDibArray.begin(),
              m_TwainDibArray.end(),
              Dib);
    if ( it != m_TwainDibArray.end() )
    {
        (*it)->Delete();
        return true;
    }
    return false;
}

bool CTL_TwainDibArray::DeleteDibMemory(size_t nWhere )
{
    m_TwainDibArray[nWhere]->Delete();
    return true;
}


bool CTL_TwainDibArray::DeleteDibMemory(HANDLE hDib )
{
    vector<CTL_TwainDibPtr>::iterator it;
    it = find_if(m_TwainDibArray.begin(),
                 m_TwainDibArray.end(),
                 FindHandlePred(hDib) );

    if ( it != m_TwainDibArray.end() )
    {
        (*it)->Delete();
        return true;
    }
    return false;
}
