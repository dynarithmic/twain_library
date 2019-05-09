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
#ifndef GIFFUN_H_
#define GIFFUN_H_

#include "winbit.h"

#ifdef  WIN32
    #ifdef __BORLANDC__
        #pragma option -a1
    #else
        #pragma pack (push, before_gif)
        #pragma pack (1)
    #endif
#else
#endif

typedef struct {
    char sig[6];
    unsigned short int screenwidth,screendepth;
    unsigned char flags,background,aspect;
    } GIFHEADER;

typedef struct {
    unsigned short int left,top,width,depth;
    unsigned char flags;
    } GIFIMAGEBLOCK;

typedef struct {
    char blocksize;
    char flags;
    unsigned short int delay;
    char transparent_colour;
    char terminator;
    } GIFCONTROLBLOCK;

typedef struct {
    char blocksize;
    unsigned short int left,top;
    unsigned short int gridwidth,gridheight;
    char cellwidth,cellheight;
    char forecolour,backcolour;
    } GIFPLAINTEXT;

typedef struct {
    char blocksize;
    char applstring[8];
    char authentication[3];
    } GIFAPPLICATION;

#ifdef  WIN32
    #ifdef __BORLANDC__
        #pragma option -a.
    #else
        #pragma pack (pop, before_gif)
    #endif
#else   /* WIN32 */
#endif  /* WIN32 */

class CGIFImageHandler : public CDibInterface
{
    public:
        CGIFImageHandler();
        // Virtual interface
        virtual LPSTR   GetFileExtension() const;
        virtual HANDLE  GetFileInformation(LPCSTR path) ;
        virtual HANDLE  ReadGraphicFile(LPCSTR path) ;
        virtual WORD    WriteGraphicFile(LPCSTR path, HANDLE bitmap, BOOL bUsefh=FALSE,
                                         int fhToUse = 0, LONG Info=0);

        //bitmap library
/*        virtual WORD    CopyPictureToClipboard(HWND hwnd,HANDLE hsource) ;
        virtual HANDLE  PastePictureFromClipboard(HWND hwnd) ;
        virtual WORD    PrintBitmap(HDC hpr,HANDLE handle) ;
        virtual HANDLE  Dither256(HANDLE hsource) ;
        virtual HANDLE  Dither16(HANDLE hsource) ;
        virtual HANDLE  Dither2(HANDLE hsource) ;
        virtual WORD    ShowDib(HDC hdc,HANDLE handle,WORD x,WORD y) ;
  */
    private:
        void    GIFDoSkipExtension(int fh);
        int     GIFDoUnpackImage(int fh,int bits,LPBITMAPINFOHEADER lpbi,int flags);
        int     GIFWriteScreenDesc(int fh,LPBITMAPINFOHEADER lpbi,LPSTR sig);
        int     GIFWriteImageDesc(int fh,LPBITMAPINFOHEADER lpbi);
        int     GIFCompressImage(int fh,LPBITMAPINFOHEADER lpbi);
        int     GIFWriteComment(int fh,LPSTR comment);
        void    GIFInitTable(int min_code_size);
        void    GIFWriteCode(int fh,int code);
        int     GIFFlush(int fh,int n);


        LPINT   m_poldcode;
        LPINT   m_pcurrentcode;
        LPSTR   m_pnewcode;
        BOOL    m_bWriteGif89;
        int     m_bit_offset;
        int     clear_code;
        int     free_code;
        int     max_code;
        int     code_size;
        int     eof_code;
        int     byte_offset;
        int     bit_offset;
        int     bits_left;
        char    code_buffer[259];

};

#endif
