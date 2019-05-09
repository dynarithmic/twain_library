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
#ifndef DTWAIN_PDFLIB_H
#define DTWAIN_PDFLIB_H

#pragma message ("In dtwainpdf.h")
// define the main class
#include <string>
#include <fstream>
#include <list>
#include <map>
#include <set>
#include <vector>
#include <dtwainc.h>
#include <pdfencrypt.h>
#include <memory>
#include "pdffont_basic.h"
#include "ctlobstr.h"
#include "tiff.h"
#include "tiffio.h"

namespace dynarithmic
{
    #ifdef NO_CONST_KEYS
        typedef std::map< int, std::string > MediaBoxMap;
    #else
        typedef std::map< const int, std::string > MediaBoxMap;
    #endif

    typedef std::map< unsigned int, PDFFont> FontRefToFontInfoMap;
    typedef std::map< CTL_StringType, PDFFont> FontNameToFontInfoMap;
    typedef std::map< unsigned int, PDFFont> FontNumberToFontInfoMap;
    typedef std::set< std::string > StringSet;
    typedef std::vector< std::string > StringVector;

    class PdfDocument;

    class PDFObject
    {
        private:
            int m_nObjNum;
            std::string Contents;
            unsigned long m_byteOffset;
            PdfDocument *m_pParentDoc;
            bool m_bIsEncrypted;
            bool m_bASCIIHexCompression;

        public:
            PDFObject(int objNum=-1) : m_nObjNum(objNum), m_pParentDoc(NULL),
                        m_bIsEncrypted(false), m_bASCIIHexCompression(false) { }
            virtual ~PDFObject() { }
            void SetByteOffset(unsigned long byteoffset) { m_byteOffset = byteoffset; }
            void SetASCIIHexCompression(bool bSet) { m_bASCIIHexCompression = bSet; }
            bool IsASCIICompressed() { return m_bASCIIHexCompression; }
            void AssignParent(PdfDocument *pDoc) {m_pParentDoc = pDoc; }
            PdfDocument* GetParent() const { return m_pParentDoc; }
            void SetEncrypted(bool bEncrypted=true) { m_bIsEncrypted = bEncrypted; }
            bool IsEncrypted() const { return m_bIsEncrypted; }
            int EncryptBlock(const std::string &sIn, std::string& sOut, int objectnum, int gennum);
            int EncryptBlock(char *pIn, int nLength, int objectnum, int gennum);
            virtual std::string GetExtraInfo() { return ""; }
            virtual std::string GetExtraInfoEnd() { return ""; }
            virtual std::string GetStreamContents() { return GetContents(); }

        public:
            int GetObjectNum() const { return m_nObjNum; }
            void SetObjectNum(int nNum) { m_nObjNum = nNum; }

            std::string GetContents() const { return Contents; }
            void SetContents(const std::string& s) {Contents = s; }
            void AppendContents(const std::string& s) {Contents += s; }
            void WriteRaw(const char *pBuffer, size_t nLength)
            {
                Contents.append(pBuffer, nLength);
            }
            int GetContentsSize() const { return (int)Contents.length(); }

            virtual void ComposeObject() { }
    };


    class CatalogObject : public PDFObject
    {
        public:
            CatalogObject() : PDFObject(1){ }
            void ComposeObject()
            {
                SetContents("<< /Type /Catalog\r\n/Pages 2 0 R\r\n>>");
            }
    };

    class PagesObject : public PDFObject
    {
        public:
            PagesObject() : PDFObject(2) { KidsArrayObjects.reserve(100);}
            void AddObjectToKids(int nNum) { KidsArrayObjects.push_back(nNum); }
            void ComposeObject();

        private:
            std::vector<int> KidsArrayObjects;
    };

    class ContentsObject : public PDFObject
    {
        public:
            ContentsObject(int objnum) : PDFObject(objnum) { }
            void SetImageName(const std::string& sImgName) { m_sImgName = sImgName; }
            void SetScaling(double x, double y) { m_xscale = x; m_yscale = y; }
            void ComposeObject();
            void PreComposeObject();

            std::string GetStreamContents();
            std::string GetExtraInfo();
            std::string GetExtraInfoEnd();
            std::string GetPreComposedString() const { return m_preComposedObject; }
            void CreateFontDictAndText(int startObjNum, int& nextObjNum);
            std::string GetFontDictionaryString() const { return m_sFontDictString; }

        private:
            std::string m_sImgName;
            std::string m_sContents;
            std::string m_sExtraInfo;
            std::string m_sExtraInfoEnd;
            std::string m_preComposedObject;
            std::string m_sFontString;
            std::string m_sFontDictString;

            double m_xscale;
            double m_yscale;
    };

    typedef std::map<unsigned long, unsigned long> CRCMapToObj;
    typedef std::map<unsigned long, unsigned long> CRCMapToPage;

    class ImageObject : public PDFObject
    {
        public:
            ImageObject(int objnum, const CTL_StringType& sImgName) : PDFObject(objnum), m_sImgName(sImgName) { }
            void SetPDFImageName(const std::string& sPDFImgName) { m_sPDFImgName = sPDFImgName; }
            bool OpenAndComposeImage(int& width, int& height, int& bpp, int& rgb, int& dpix, int& dpiy);
            void ComposeObject();
            unsigned long GetCRCVal() const { return m_nCurCRCVal; }
            static std::vector<char>& GetDataStream() { return m_vImgStream; }
            std::string GetStreamContents();
            std::string GetExtraInfo();
            std::string GetExtraInfoEnd();

        protected:
            bool ProcessJPEGImage(int& width, int& height, int& bpp, int& rgb);
            bool ProcessTIFFImage(int& width, int& height, int& bpp, int& rgb, int&dpix, int& dpiy);
            bool ProcessBMPImage(int& width, int& height, int& bpp, int& rgb, int& dpix, int& dpiy);

            static tsize_t libtiffReadProc (thandle_t fd, tdata_t buf, tsize_t size);
            static tsize_t libtiffWriteProc(thandle_t fd, tdata_t buf, tsize_t nsize);
            static toff_t  libtiffSeekProc (thandle_t fd, toff_t off, int i);
            static int     libtiffCloseProc (thandle_t fd);
            static toff_t  libtiffSizeProc(thandle_t fd);

        private:
            CTL_StringType m_sImgName;
            std::string m_sPDFImgName;
            int  m_bpp;
            std::string m_sColorSpace;
            std::string m_sImageInfo;
            std::string m_sExtraInfo;
            std::string m_sExtraInfoEnd;
            int m_Width;
            int m_Height;
            size_t m_imgLengthInBytes;
            static std::vector<char> m_vImgStream;
            static unsigned int m_sTiffOffset;
            unsigned long m_nCurCRCVal;
            int m_nTiffKValue;
            int m_nImgType;
            int m_nTiffColumns;
            int m_nTiffRows;
    };

    class InfoObject : public PDFObject
    {
        public:
            InfoObject(int objnum) : PDFObject(objnum),
            m_sProducer("(None)"),
            m_sAuthor("(None)"),
            m_sTitle("(None)"),
            m_sSubject("(None)"),
            m_sKeywords("(None)"),
            m_sCreator("(None)")
            { }

            void ComposeObject();
            void SetProducer(const std::string& s) { m_sProducer = s; }
            void SetAuthor(const std::string& s) { m_sAuthor = s; }
            void SetTitle(const std::string& s) { m_sTitle = s; }
            void SetSubject(const std::string& s) { m_sSubject = s; }
            void SetKeywords(const std::string& s) { m_sKeywords = s; }
            void SetCreator(const std::string& s) { m_sCreator = s; }

        private:
            std::string m_sProducer, m_sAuthor, m_sTitle, m_sSubject, m_sKeywords,
                                      m_sCreator;
            std::string m_sDate;
    };

    class EncryptionObject : public PDFObject
    {
        public:
            EncryptionObject(int objnum) : PDFObject(objnum) , m_RValue(2), m_bAESEncrypted(false) { }
            void ComposeObject();
            void SetRValue(int RValue) { m_RValue = RValue; }
            void SetLength(int nLength) { m_nLength = nLength; }
            void SetFilter(const std::string& sFilter) { m_sFilter = sFilter; }
            void SetOwnerPassword(const std::string& oValue) {m_sOwnerValue = oValue; }
            void SetUserPassword(const std::string& pValue) {m_sUserValue = pValue; }
            void SetVValue(int VValue) { m_nVValue = VValue; }
            void SetPermissions(int nPermissions) { m_nPermissions = nPermissions; }
            void SetAESEncryption(bool bSet) { m_bAESEncrypted = bSet; }

        private:
            int m_RValue;
            int m_nLength;
            int m_nVValue;
            int m_nPermissions;
            bool m_bAESEncrypted;
            std::string m_sFilter;
            std::string m_sOwnerValue;
            std::string m_sUserValue;
    };

    class ProcSetObject : public PDFObject
    {
        public:
            ProcSetObject(int objnum) : PDFObject(objnum) { }
            void ComposeObject();
    };

    class FontObject : public PDFObject
    {
        public:
            FontObject(int objnum) : PDFObject(objnum) { }
            void ComposeObject();
            void SetFontName(const std::string& name) { fontname = name; }
            std::string GetFontName() const { return fontname; }

        private:
            std::string fontname;
    };

    class PageObject : public PDFObject
    {
        public:
            PageObject() { }
            PageObject(int objnum) : PDFObject(objnum),
                                     m_smediabox("[0 0 612 792]"),
                                     m_orientation(DTWAIN_PDF_PORTRAIT),
                                     m_bThumbnailImage(false)
            { }
            void SetMediaBox(const std::string& mediabox) { m_smediabox = mediabox;}
            void SetResourceObjectNum(int nNum) { m_resObjNum = nNum; }
            int  GetResourceObjectNum() const { return m_resObjNum; }
            void SetCurrentImageNum(unsigned num) { m_nImageNum = num; }
            void SetOrientation(int orientation) { m_orientation = orientation; }
            void SetDuplicateImage(bool bIsDuplicate, unsigned long nObjNum)
            {
                m_bDuplicateImage = bIsDuplicate;
                m_nDuplicateObjNum = nObjNum;
            }
            void SetDuplicateThumbnailImage(bool bIsDupThumb, unsigned long nObjNum)
            {
                m_bDuplicateThumbImage = bIsDupThumb;
                m_nDuplicateThumbObjNum = nObjNum;
            }
            unsigned long GetDuplicateImageNum() const { return m_nDuplicateObjNum; }
            unsigned long GetDuplicateThumbnailNum() const { return m_nDuplicateThumbObjNum; }
            void ComposeObject();
            unsigned long GetCRCValue() const { return m_CRCValue; }
            void EnableThumbnailImage(bool bEnable=true) { m_bThumbnailImage = bEnable; }
            unsigned long GetMaxObjectNum() const { return m_nMaxObjectNum; }
            void SetContentsObject(ContentsObject* curContents) { theContents = curContents; }

        private:
            std::string m_smediabox;
            ContentsObject* theContents;
            int m_resObjNum;
            unsigned long m_nImageNum;
            int m_orientation;
            bool m_bDuplicateImage;
            bool m_bDuplicateThumbImage;
            bool m_bThumbnailImage;
            unsigned long m_nDuplicateObjNum;
            unsigned long m_nDuplicateThumbObjNum;
            unsigned long m_CRCValue;
            unsigned long m_nMaxObjectNum;
    };

    struct ObjectInfo
    {
        unsigned long ObjNum;
        unsigned long ObjOffset;
    };


    #define OWNER_PASSWORD  0
    #define USER_PASSWORD   1
    #define START_FONTREF_NUM 1

    class PdfDocument
    {
        public:
            enum PaperSize { US_LETTER, LEGAL, A4 };
            enum CompressTypes { NO_COMPRESS, FLATE_COMPRESS, A85_COMPRESS, AHEX_COMPRESS };
            PdfDocument();
            ~PdfDocument();

            // Set the PDF version
            void SetPDFVersion(const std::string& pdfVer) { m_sPDFVer = pdfVer; }
            void SetPDFVersion(int major, int minor);
            std::string GetPDFVersion() const { return m_sPDFVer; }

            // Set the paper size
            // Set the media box directly
    //        void SetMediaBox(const std::string& sMediaBox) { m_curmediabox = sMediaBox; }

            // Set the PDF header (binary) data
            void SetPDFHeader(const std::string& sHeader) { m_sPDFHeader = sHeader; }
            std::string GetPDFHeader() const { return m_sPDFHeader; }

            bool OpenNewPDFFile(const CTL_StringType& sFile);
            void SetPolarity(int PolarityType) { m_nPolarity = PolarityType; }
            int GetPolarity() const { return m_nPolarity; }
            bool SetImageFileName(const std::string& sImgName);
            bool WritePage(const CTL_StringType& sFileName);
            void SetMediaBox(int mediatype);
            void SetMediaBox(const std::string& sMedia) { m_smediabox = sMedia; }
            void SetOrientation( int oType ) { m_Orientation = oType; }
            void SetScaling(double xscale, double yscale) {m_xscale = xscale; m_yscale = yscale;}
            void SetScaleType(int scaletype) { m_scaletype = scaletype; }
            bool StartPDFCreation();
            bool EndPDFCreation();
            bool IsDuplicateImage(unsigned long CRCVal, unsigned long& ObjNum);
            void AddDuplicateImage(unsigned long CRCVal, unsigned long nObjNum);
            bool IsDuplicatePage(unsigned long CRCVal, unsigned long& ObjNum);
            void AddDuplicatePage(unsigned long CRCVal, unsigned long nObjNum);
            void SetCompression(bool bSet=true) {m_bCompression = bSet; }
            bool GetCompression() const { return m_bCompression; }
            void SetProducer(const std::string& s) { m_sProducer = s; }
            void SetAuthor(const std::string& s) { m_sAuthor = s; }
            void SetTitle(const std::string& s) { m_sTitle = s; }
            void SetSubject(const std::string& s) { m_sSubject = s; }
            void SetKeywords(const std::string& s) { m_sKeywords = s; }
            void SetCreator(const std::string& s) { m_sCreator = s; }

            // Searchable text added to PDF file
            void SetSearchableText(const std::string& s);
            std::string GetSearchableText() const { return m_SearchText; }
            unsigned long GetProcSetObjNum() const { return m_nProcSetObj; }
            void SetProcSetObjNum(unsigned long num) { m_nProcSetObj = num; }
            bool IsProcSetObjDone() { return m_bProcSetObjEstablished; }
            void SetProcSetObjDone(bool bSet) { m_bProcSetObjEstablished = bSet; }

            unsigned long GetFontObjNum() const { return m_nFontObj; }
            void SetFontObjNum(unsigned long num) { m_nFontObj = num; }
            bool IsFontObjDone() { return m_bFontObjEstablished; }
            void SetFontObjDone(bool bSet) { m_bFontObjEstablished = bSet; }

            void SetImageType(int nImageType) { m_nImageType = nImageType; }
            int  GetImageType() const { return m_nImageType; }
            void SetThumbnailFile(const CTL_StringType& s) { m_sThumbnailFileName = s; }
            void SetDPI(unsigned long dpi) { m_dpi = dpi; }
            void SetASCIICompression(bool bSet) { m_bASCIICompression = bSet; }
            bool IsASCIICompressed() { return m_bASCIICompression; }
            bool IsNoCompression() const { return m_bIsNoCompression; }
            void SetNoCompression(bool bSet) { m_bIsNoCompression = bSet; }
            unsigned long GetDPI() const { return m_dpi; }
            CTL_StringType GetThumbnailFile() const { return m_sThumbnailFileName; }
            PDFEncryption& GetEncryptionEngine() { return *m_Encryption.get(); }

            void SetEncryption(const CTL_StringType& ownerPassword,
                               const CTL_StringType& userPassword,
                               unsigned int permissions,
                               bool bIsStrongEncryption,
                               bool isAESEncrypted);
            bool IsEncrypted() const { return m_bIsEncrypted ;} //& !m_bASCIIHexCompression; }
            bool IsAESEncrypted() const { return m_bIsAESEncrypted; } //& !m_bASCIIHexCompression; }
            StringVector& GetDocumentID() { return m_DocumentID; }
            CTL_TEXTELEMENTNAKEDPTRLIST::const_iterator GetFirstTextElement() { return m_vPDFText.begin(); }
            void AddTextElement(PDFTextElement* element)
            {
    //            m_vPDFTextSet.insert(element);
                m_vPDFText.push_back(element);
    /*
                CTL_TEXTELEMENTNAKEDPTRSET::iterator it = m_vPDFTextSet.find(element);
                if ( it == m_vPDFTextSet.end())
                {
                    m_vPDFTextSet.insert(element);
                    m_vPDFText.push_back(element);
                }
                else
                    *(*it) = *element;*/
            }
            int GetNumTextElements() const { return (int)m_vPDFText.size(); }
            void RemoveTextElement();
            void ChangeTextElement(int nWhich);
            void CreateFontNumbersFromTextElements();
            void WriteAllFontObjects();
            int GetCurrentPageNumber() const {return m_nCurPage;}
            std::string GenerateFontDictionary(int objectNum, int& nextObjNum);
            unsigned int GetMaxFontRefNumber() const;
            void RemoveTempTextElements();

        protected:
            bool WriteHeaderInfo();
            bool WriteObject(PDFObject* pObj);
            std::string GetBinaryHeader() const;
            void SortObjects();
    //        bool IsTextElementOnPage(unsigned int tElement) const;
            bool IsTextElementOnPage(CTL_TEXTELEMENTNAKEDPTRLIST::const_iterator it) const;

        private:
            std::ofstream m_outFile;
            unsigned long m_byteOffset;
            std::string m_sPDFVer;
            std::string m_sPDFHeader;
            std::string s_ImageName;
            CTL_StringType m_sThumbnailFileName;
            std::string m_sCurSysTime;
            std::string m_sCreator;

            CatalogObject m_Catalog;
            int m_nPolarity;
            unsigned int m_nCurContentsObj;
            unsigned int m_nCurObjNum;
            std::vector<PageObject> m_vPage;
            std::vector<ObjectInfo> m_vAllOffsets;
    //        CTL_TEXTELEMENTNAKEDPTRSET  m_vPDFTextSet;
            CTL_TEXTELEMENTNAKEDPTRLIST m_vPDFText;
            unsigned int m_nCurPage;
            PagesObject m_pagesObj;
            std::string m_smediabox;
            int m_Orientation;
            MediaBoxMap m_mediaMap;
            double m_xscale;
            double m_yscale;
            int m_scaletype;
            int m_dpi;
            CRCMapToObj m_allCRC;
            CRCMapToPage m_allPageCRC;
            bool m_bCompression;
            bool m_bUseVariableMediaBox;
            std::string m_sProducer, m_sAuthor, m_sTitle, m_sSubject, m_sKeywords;
            unsigned long m_nProcSetObj;
            unsigned long m_nFontObj;
            bool          m_bProcSetObjEstablished;
            bool          m_bFontObjEstablished;
            int         m_nImageType;
            CTL_StringType m_sOutputFileName;
            StringVector m_DocumentID;
            std::shared_ptr<PDFEncryption> m_Encryption;
            StringVector m_EncryptionPassword;
            int          m_nPermissions;
            bool         m_bIsStrongEncryption;
            bool         m_bIsAESEncrypted;
            bool         m_bIsEncrypted;
            bool        m_bASCIICompression;
            bool        m_bIsNoCompression;
            std::string m_SearchText;
            FontRefToFontInfoMap m_mapFontRef;
            FontNameToFontInfoMap m_mapFontNames;
            FontNumberToFontInfoMap m_mapFontNumbers;
            int         CurFontRefNum;

    };
}
#endif

