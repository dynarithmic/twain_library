/*
    This file is part of the Dynarithmic TWAIN Library (DTWAIN).
    Copyright (c) 2002-2021 Dynarithmic Software.

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
#ifndef PDFFONT_BASIC_H_
#define PDFFONT_BASIC_H_

#include <string>
#include <list>
#include <unordered_set>
#include <memory>
#include "dtwpdft.h"
#include "ctlobstr.h"
namespace dynarithmic
{
    class CTL_ITwainSource;

    struct PDFFont
    {
        CTL_StringType m_fontName;
        int refNum;
        int fontNum;
        bool bUsedOnPage;
        PDFFont(const CTL_StringType& fname= _T("Helvetica"), int rNum = -1,
                int fNum = -1 ) :
                m_fontName(fname), refNum(rNum), fontNum(fNum), bUsedOnPage(false) { }
        bool isCreated() const { return refNum != -1; }
        bool isUsedOnPage() const { return bUsedOnPage; }
        void setUsedOnPage(bool bSet) { bUsedOnPage = bSet; }
    };

    struct PDFTextElement
    {
        PDFFont m_font;
        double xpos, ypos;
        double charSpacing;
        double wordSpacing;
        double scaling;
        double fontSize;
        int renderMode;
        int riseValue;
        int colorRGB;
        int displayFlags;
        int strokeWidth;
        unsigned stockPosition;
        double scalingX;
        double scalingY;
        double rotationAngle;
        double skewAngleX;
        double skewAngleY;
        CTL_ITwainSource *pTwainSource; // the source that owns this text element
        bool hasBeenDisplayed;
        unsigned int textTransform;
        bool isEnabled;
        CTL_StringType m_text;
        PDFTextElement() : xpos(0), ypos(0), charSpacing(0),
            wordSpacing(0), scaling(100), fontSize(10),
            renderMode(0), riseValue(0), colorRGB(0), displayFlags(0), strokeWidth(2),
            stockPosition(0), scalingX(1), scalingY(1), rotationAngle(0),
            skewAngleX(0), skewAngleY(0), pTwainSource(0), hasBeenDisplayed(false),
            textTransform(DTWAIN_PDFTEXTTRANSFORM_TSRK), isEnabled(true) { }
            std::string GetPDFTextString() const;
            void SetInvisible() { renderMode = 3; m_font.refNum = 1; }
    };

    typedef std::shared_ptr<PDFTextElement> PDFTextElementPtr;
    typedef std::list<PDFTextElementPtr> CTL_TEXTELEMENTPTRLIST;
    typedef std::list<PDFTextElement*> CTL_TEXTELEMENTNAKEDPTRLIST;
    typedef std::unordered_set<PDFTextElement*> CTL_TEXTELEMENTNAKEDPTRSET;
    typedef std::pair<CTL_TEXTELEMENTPTRLIST::iterator, CTL_TEXTELEMENTPTRLIST::iterator> CTL_SEARCHABLETEXTRANGE;
    typedef std::pair<CTL_TEXTELEMENTNAKEDPTRLIST::iterator,
                                    CTL_TEXTELEMENTNAKEDPTRLIST::iterator> CTL_SEARCHABLENAKEDTEXTRANGE;
}
#endif
