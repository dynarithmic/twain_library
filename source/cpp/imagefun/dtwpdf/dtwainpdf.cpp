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
#pragma warning (disable:4786 4702)
#endif
#define PROTOTYPES 1
#define HAVE_PROTOTYPES
#ifdef _MSC_VER
#pragma warning (push)
#pragma warning (disable : 4267 4100 4244)
#pragma warning (pop)
#endif
#include <sstream>
#include <iomanip>
#include <algorithm>
#include <vector>
#include <array>
#include <unordered_map>
#include <time.h>
#include <boost/functional/hash.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>
#include <boost/thread/thread_time.hpp>

#include "dtwainpdf.h"
#include "crc32_aux.h"
#include "jpeglib.h"
#include "winbit32.h"
#undef Z_PREFIX
#include "zlib.h"
#ifdef __MSL__
   #include <ctime>
#else
   #include <sys/timeb.h>
#endif
#ifdef _MSC_VER
#pragma warning (push)
#pragma  warning (disable : 4702)
#endif
#include <md5-global.h>
#include <md5c.h>
#include "a85encode.h"
#include "ahexencode.h"
#include "flateencode.h"
#include "pdfencrypt.h"
#include "dtwain_verinfo.h"
#include "tiffio.h"
#ifdef _MSC_VER
#pragma warning (pop)
#endif
#undef min
#undef max
#define D_TO_R_SCALEFACTOR (3.141529654 / 180.0)
#define DegreesToRadians(x) ((x) * D_TO_R_SCALEFACTOR)

using namespace std;
using namespace dynarithmic;

// Add DTWAIN stuff if internal to DTWAIN32.DLL
#ifdef PDFLIB_INTERNAL
    #define IMGFUNC_IGNORE
    #include "ctltwmgr.h"

    #define WRITE_TO_LOG()
#else
    #define WRITE_TO_LOG()
#endif

#define FLOAT_DELTA  (+1.0e-8)
#define FLOAT_CLOSE(x,y) (fabs((x) - (y)) <= FLOAT_DELTA)
#define EXTRA_OBJECTS   3

vector<unsigned char> MD5Hash (unsigned char *input);
static string GetPDFDate();
static std::string CreateIDString(const std::string& sName, std::string& ID1, std::string& ID2);
static std::string HexString(unsigned char *input, int length=-1);
static std::string MakeCompatiblePDFString(const std::string& sString);
std::string GetSystemTimeInMilliseconds();
static bool IsRenderModeStroked(int rendermode);

static int EncodeVectorStream(const vector<char>& InputStream,
                                size_t InputLength,
                                vector<char>& OutStream,
                                PdfDocument::CompressTypes compresstype= PdfDocument::FLATE_COMPRESS);

typedef std::array<std::array<double, 3>, 3> Matrix3_3;

static Matrix3_3 MultiplyMatrix33(const Matrix3_3& m1, const Matrix3_3& m2)
{
    Matrix3_3 product = {0,0,0,0,0,0,0,0,0};
    for (int row = 0; row < 3; row++)
    {
        for (int col = 0; col < 3; col++)
        {
            // Multiply the row of A by the column of B to get the row, column of product.
            for (int inner = 0; inner < 2; ++inner)
                product[row][col] += m1[row][inner] * m2[inner][col];
        }
    }
    return product;
}


tsize_t ImageObject::libtiffReadProc (thandle_t /*fd*/, tdata_t /*buf*/, tsize_t /*size*/)
{
  // Return the amount of data read, which we will always set as 0 because
  // we only need to be able to write to these in-memory tiffs
  return 0;
}


vector<char> ImageObject::m_vImgStream;
unsigned int ImageObject::m_sTiffOffset;
tsize_t ImageObject::libtiffWriteProc (thandle_t /*fd*/, tdata_t buf, tsize_t nsize)
{
    // libtiff will try to write an 8 byte header into the tiff file. We need
    // to ignore this because PDF does not use it...
    if ((nsize == 8) && (((char *) buf)[0] == 'I') && (((char *) buf)[1] == 'I')
      && (((char *) buf)[2] == 42))
    {
      // Skip the header -- little endian
    }
    else if ((nsize == 8) && (((char *) buf)[0] == 'M') &&
       (((char *) buf)[1] == 'M') && (((char *) buf)[2] == 42))
    {
      // Skip the header -- big endian
    }
    else
    {

        // Have we done anything yet?
        if ( m_vImgStream.size() == 0 )
            m_vImgStream.resize( nsize );

        // Otherwise, we need to grow the memory buffer
        else
        {
            m_vImgStream.resize( nsize + m_sTiffOffset );
        }

        // Now move the image data into the buffer
        memcpy (&m_vImgStream[m_sTiffOffset], buf, nsize);
        m_sTiffOffset += static_cast<unsigned>(nsize);
    }
    return (nsize);
}


toff_t ImageObject::libtiffSeekProc (thandle_t /*fd*/, toff_t off, int /*i*/)
{
  // This appears to return the location that it went to
  return off;
}

int ImageObject::libtiffCloseProc (thandle_t /*fd*/)
{
  // Return a zero meaning all is well
  return 0;
}

toff_t ImageObject::libtiffSizeProc (thandle_t /*fd*/)
{
    // Return a zero meaning all is well
    return 0;
}

// Helper functions
// This is the MD5 function
vector<unsigned char> MD5Hash (unsigned char *input)
{
  MD5_CTX context;
  vector<unsigned char> digest(16);

  MD5Init (&context);
  MD5Update (&context, input, (unsigned int)strlen((const char *)input));
  MD5Final (digest.data(), &context);

  return digest;
}

std::string HexString(unsigned char *input, int length/*=-1*/)
{
    std::string sOut;
    if ( length == -1 )
        length = (int)strlen((const char *)input);
    ASCIIHexEncode(std::string((const char *)input, length), sOut);
    if (!sOut.empty())
        sOut.pop_back();
    return sOut;
}

std::string CreateIDString(const std::string& sName, std::string& ID1, std::string& ID2)
{
    char szBuf[1024];

    std::string sNow = GetPDFDate();
    WRITE_TO_LOG()
    sprintf(szBuf, "%s-%s", sNow.c_str(), sName.c_str());
    WRITE_TO_LOG()
    vector<unsigned char> hash = MD5Hash((unsigned char *)szBuf);
    hash.resize(32,'\0');
    WRITE_TO_LOG()
    vector<unsigned char> version = MD5Hash((unsigned char *)"001");
    version.resize(32,'\0');
    WRITE_TO_LOG()
    std::string hexHash;
    WRITE_TO_LOG()
    hexHash.append(HexString(hash.data(), 32).data(), 32);
    WRITE_TO_LOG()
    std::string hexVersion;
    WRITE_TO_LOG()
    hexVersion.append(HexString(version.data(), 32).data(), 32);
    WRITE_TO_LOG()

    sprintf(szBuf, "[<%s> <%s>]", hexHash.c_str(), hexVersion.c_str());
    WRITE_TO_LOG()
    ID1 = hexHash;
    WRITE_TO_LOG()
    ID2 = hexVersion;
    WRITE_TO_LOG()
    return szBuf;
}

static string MakeLandscapeMediaBox(const string& sBox)
{
    double f1[4];
    char szBuf[50];
    string sStart = sBox.substr(1, sBox.length() - 2);
    sscanf(sStart.c_str(), "%lf%lf%lf%lf", &f1[0], &f1[1], &f1[2], &f1[3]);
    sprintf(szBuf, "%-5.2lf %-5.2lf %-5.2lf %-5.2lf", f1[0], f1[1], f1[3], f1[2]);
    sStart = "[";
    sStart += szBuf;
    sStart += "]";
    return sStart;
}

std::string MakeCompatiblePDFString(const PDFEncryption::UCHARArray& u)
{
    std::string sTemp((const char *)u.data(), u.size());
    return MakeCompatiblePDFString(sTemp);
}

std::string MakeCompatiblePDFString(const std::string& sString)
{
    // Search for forward slash and replace with two forward slashes
    std::string sNew;
    sNew.reserve(100);
    string::difference_type nEscapes[5] = {0};
    unsigned char nChars[] = { 0x09, 0x0d, 0x0a, 0x0c, 0x08 };

    PDFEncryption::UCHARArray nEscapeChar(5);
    std::copy(nChars, nChars + 5, nEscapeChar.begin());

    const char *nEscapeString[] = { "\\t", "\\r", "\\n", "\\f", "\\b" };

    string::difference_type nForward = std::count(sString.begin(), sString.end(), '\\');
    string::difference_type sum = 0;

    for ( int i = 0; i < 5; ++i )
    {
        nEscapes[i] = std::count(sString.begin(), sString.end(), nEscapeChar[i]);
        sum += nEscapes[i];
    }

    if ( nForward + sum > 0 )
    {
        std::string::const_iterator it1 = sString.begin();
        std::string::const_iterator it2 = sString.end();
        PDFEncryption::UCHARArray::iterator found;

        bool addit;
        while (it1 != it2 )
        {
            addit = true;
            if ( *it1 == '\\' )
                sNew += '\\';
            else
            {
                found = std::find(nEscapeChar.begin(), nEscapeChar.end(), *it1);

                if ( found != nEscapeChar.end() )
                {
                    int dist = (int)std::distance(nEscapeChar.begin(), found);
                    sNew += nEscapeString[dist];
                    addit = false;
                }
            }
            if ( addit )
                sNew += *it1;
            ++it1;
        }
    }
    else
        sNew = sString;

    // balances parentheses
    int nLeft = (int)std::count(sNew.begin(), sNew.end(), '(');
    int nRight = (int)std::count(sNew.begin(), sNew.end(), ')');
    if ( nLeft == nRight )
        return sNew;

    // prepend all parens with / characters
    std::string sNew2;
    std::string::const_iterator it3 = sNew.begin();
    std::string::const_iterator it4 = sNew.end();
    while (it3 != it4 )
    {
        if (*it3 == '(' || *it3 == ')')
            sNew2 += "\\";
        sNew2 += *it3;
        ++it3;
    }
    return sNew2;
}

static int NoCompress(const std::string& inData, std::string& outData)
{
	outData = inData;
	return 1;
}

static int EncodeVectorStream(const vector<char>& InputStream,
                              size_t InputLength,vector<char>& OutStream,PdfDocument::CompressTypes compresstype)
{

    static std::unordered_map<PdfDocument::CompressTypes, std::function<int(const std::string&, std::string&)>>
                compress_fn = { { PdfDocument::NO_COMPRESS, ::NoCompress },
								{ PdfDocument::A85_COMPRESS, ::ASCII85Encode },
                                { PdfDocument::AHEX_COMPRESS, ::ASCIIHexEncode },
                                { PdfDocument::FLATE_COMPRESS, ::FlateEncode}, };

        auto fnCall = compress_fn.find(compresstype);
        if (fnCall != compress_fn.end())
        {
            std::string sTemp;
            std::string sOut;
            sTemp.append(InputStream.data(), InputLength);
            fnCall->second(sTemp, sOut);
            OutStream.resize(sOut.size());
            std::copy(sOut.begin(), sOut.end(), OutStream.begin());
            return 1;
    }
    return 0;
}

static string MakeDate(int year, int month, int day, int hour, int minutes,
                            int seconds)
{

    // Get current utc time
    boost::posix_time::ptime timeutc =
        boost::posix_time::second_clock::universal_time();

    // get time zone difference
    auto utchour_diff = hour - timeutc.time_of_day().hours();

    char negValue = '+';
    if ( utchour_diff < 0 )
        negValue = '-';
    utchour_diff = abs(utchour_diff);
    auto utcminute_diff = minutes - timeutc.time_of_day().minutes();
    if ( utcminute_diff < 0 )
        utcminute_diff = 0;

    // Make the string
    char szBuf[255];
    sprintf(szBuf, "D:%4d%02d%02d%02d%02d%02d%c%02d'%02d'", year, month, day, hour, minutes, seconds, negValue, static_cast<int>(utchour_diff), static_cast<int>(utcminute_diff));
    return szBuf;
}

std::string GetSystemTimeInMilliseconds()
{
    boost::posix_time::ptime time_t_epoch(boost::gregorian::date(1601, 1, 1));
    auto systimex = boost::get_system_time();
    boost::posix_time::time_duration diff = systimex - time_t_epoch;
    auto mill = diff.total_milliseconds() * 10000LL;
    CTL_StringStreamA strm;
    strm << mill;
    return strm.str();
}

static string GetPDFDate()
{
    struct tm *timenow;
    time_t curtime;

    // Get the current time...
    curtime = time (NULL);
    timenow = (struct tm *) localtime (&curtime);

    return MakeDate (1900 + timenow->tm_year,
             timenow->tm_mon + 1,
             timenow->tm_mday,
             timenow->tm_hour, timenow->tm_min, timenow->tm_sec);

};


int PDFObject::EncryptBlock(const std::string& sIn, std::string& sOut, int objectnum, int gennum)
{
    PdfDocument *pParent = GetParent();
    if ( !pParent )
        return ENCRYPTION_UNAVAILABLE;

    if ( !pParent->IsEncrypted() )
        return ENCRYPTION_NOTSET;

    // Encrypt the block of code
    // Get the encryption engine from the parent
    PDFEncryption& enc = pParent->GetEncryptionEngine();
    // now get the hash key from the object and generation numbers;
    enc.SetHashKey(objectnum, gennum);
    enc.PrepareKey();
    enc.Encrypt(sIn, sOut);
    return ENCRYPTION_OK;
}

PdfDocument::PdfDocument() :
    m_byteOffset(0),
    m_sPDFVer("1.3"),
    m_sPDFHeader("DTWAIN PDF, 2019"),
    m_sCurSysTime(GetSystemTimeInMilliseconds()),
    m_nPolarity(DTWAIN_PDFPOLARITY_POSITIVE),
    m_smediabox("[0 0 612 792]"),
    m_Orientation(DTWAIN_PDF_PORTRAIT),
    m_xscale(0),
    m_yscale(0),
    m_scaletype(DTWAIN_PDF_NOSCALING),
    m_dpi(0),
    m_bCompression(false),
    m_bUseVariableMediaBox(false),
    m_sProducer("(None)"),
    m_sAuthor("(None)"),
    m_sTitle("(None)"),
    m_sSubject("(None)"),
    m_sKeywords("(None)"),
    m_nFontObj(0),
    m_bProcSetObjEstablished(false),
    m_bFontObjEstablished(false),
    m_nImageType(0),
    m_DocumentID(2),
    m_Encryption(new PDFEncryptionRC4),
    m_EncryptionPassword(2),
    m_nPermissions(0),
    m_bIsStrongEncryption(false),
    m_bIsAESEncrypted(false),
    m_bIsEncrypted(false),
    m_bASCIICompression(false),
    m_bIsNoCompression(false),
	m_nCurContentsObj(0),
	m_nCurObjNum(0),
	m_nCurPage(0),
	m_nProcSetObj(0),
    CurFontRefNum(START_FONTREF_NUM),
	m_mediaMap(CTL_TwainDLLHandle::GetPDFMediaMap())
{
	auto iter = m_mediaMap.find(DTWAIN_FS_USLETTER);
	if (iter != m_mediaMap.end())
		m_smediabox = iter->second.second;
}

void PdfDocument::SetPDFVersion(int major, int minor)
{
    ostringstream strm;
    strm << major << "." << minor;
    m_sPDFVer = strm.str();
}

char converttobin(char ch)
{
    return (char)(256 - ch);
}


string PdfDocument::GetBinaryHeader() const
{
    string sBinHeader;
    sBinHeader = m_sPDFHeader;
    transform(sBinHeader.begin(), sBinHeader.end(), sBinHeader.begin(), converttobin);
    return sBinHeader;
}

bool PdfDocument::WriteHeaderInfo()
{
    ostringstream strm;
    string sOut;
    string binheader = GetBinaryHeader();

    if ( IsASCIICompressed() )
        ASCIIHexEncode(binheader, sOut);
    else
        sOut = std::move(binheader);

    strm << "%PDF-" << GetPDFVersion() << "\n" << sOut << "\r";
    string s = strm.str();
    m_byteOffset = (int)s.length();
    m_outFile.write(s.c_str(), s.length());
    return true;
}


bool PdfDocument::OpenNewPDFFile(const CTL_StringType& sFile)
{
    WRITE_TO_LOG()
    // Open the file here
    {
        CTL_StringStreamType strm;
        strm << _T("File name to be saved: ") << sFile;
        CTL_TwainAppMgr::WriteLogInfo( strm.str() );
    }

    m_outFile.open(StringConversion::Convert_Native_To_Ansi(sFile).c_str(), ios::binary|ios::out);
    WRITE_TO_LOG()
    if ( !m_outFile )
        return false;
    WRITE_TO_LOG()
    m_sOutputFileName = sFile;
    WRITE_TO_LOG()
    // Get the document ID for this file
    CreateIDString(StringConversion::Convert_Native_To_Ansi(m_sOutputFileName), m_DocumentID[0], m_DocumentID[1]);
    WRITE_TO_LOG()
    return true;
}

void PdfDocument::SetSearchableText(const std::string& /*s*/)
{
/*    m_SearchText = s;
    // Add a text element
    PDFTextElement tElement;
    tElement.m_text = s;
    tElement.SetInvisible();
    tElement.displayFlags = DTWAIN_PDFTEXT_CURRENTPAGE;
    m_vPDFText.push_back(tElement);*/
}

bool RemoveCurrentText(const PDFTextElement* element)
{
    return ((*element).displayFlags & DTWAIN_PDFTEXT_CURRENTPAGE)?true:false;
}

void PdfDocument::RemoveTempTextElements()
{
    m_vPDFText.clear();
}

bool PdfDocument::WritePage(const CTL_StringType& sImgFileName)
{
    m_nCurPage++;
    char szBuf[100];
    sprintf(szBuf, "Img%d", m_nCurPage);
    string actualFileName = szBuf;
    PageObject page(m_nCurObjNum);
    page.AssignParent(this);

    // Create the main image
    ImageObject imgObj(m_nCurObjNum + 2, sImgFileName);
    imgObj.SetPDFImageName(szBuf);
    imgObj.AssignParent(this);
    imgObj.SetEncrypted(IsEncrypted());

    int width, height, bpp, rgb, dpix, dpiy;
    if ( !imgObj.OpenAndComposeImage(width, height, bpp, rgb, dpix, dpiy) )
        return false;

    unsigned long nObjNum = 0;

    // Check if this is a duplicate
    bool bIsDuplicate = IsDuplicateImage(imgObj.GetCRCVal(), nObjNum);
    if ( !bIsDuplicate )
    {
        // Not a duplicate object, so compose a new image object
        imgObj.ComposeObject();
    }

    // Create the thumbnail
    char szBufThumb[100];
    ImageObject imgObjThumb(m_nCurObjNum + 4, m_sThumbnailFileName);
    sprintf(szBufThumb, "ImgThumb%d", m_nCurPage);
    bool bIsDuplicateThumb = false;
    unsigned long nObjNumThumb = false;
    bool bIsThumbnail = !m_sThumbnailFileName.empty();
    if ( bIsThumbnail )
    {
        imgObjThumb.SetPDFImageName(szBufThumb);
        imgObjThumb.AssignParent(this);

        int width2, height2, bpp2, rgb2, dpix2, dpiy2;
        if ( imgObjThumb.OpenAndComposeImage(width2, height2, bpp2, rgb2, dpix2, dpiy2))
        {
            bIsDuplicateThumb = IsDuplicateImage(imgObjThumb.GetCRCVal(), nObjNumThumb);
        }
    }
    // Create the contents object
    ContentsObject cObj(m_nCurObjNum + 1);
    cObj.AssignParent(this);


    // Determine media box
    string mbox = m_smediabox;
    double realwidth = width;
    double realheight = height;
    if ( !m_bUseVariableMediaBox )
    {
        if ( m_Orientation == DTWAIN_PDF_LANDSCAPE )
            mbox = MakeLandscapeMediaBox(m_smediabox);

        if ( dpix > 1 )
        {
            realwidth = ((double)width / (double)dpix) * 72.0;
        }
        if ( dpiy > 1)
        {
            realheight = ((double)height / (double)dpiy) * 72.0;
        }

        switch(m_scaletype)
        {
            case DTWAIN_PDF_NOSCALING:
            {
                cObj.SetScaling( realwidth, realheight);
            }
            break;

            case DTWAIN_PDF_FITPAGE:
            {
                double f1[4];
                string sStart = mbox.substr(1, mbox.length() - 2);
                sscanf(sStart.c_str(), "%lf%lf%lf%lf", &f1[0], &f1[1], &f1[2], &f1[3]);
                cObj.SetScaling(f1[2], f1[3]);
            }
            break;

            case DTWAIN_PDF_CUSTOMSCALE:
            {
                if ( m_Orientation == DTWAIN_PDF_PORTRAIT)
                    cObj.SetScaling(m_xscale * realwidth,
                                    m_yscale * realheight);
                else
                    cObj.SetScaling(m_xscale * realheight,
                                    m_yscale * realwidth);

            }
        }
    }
    else
    {
        // variable page size
        // Get the media box for this page
        // check for custom scaling
        if (m_scaletype == DTWAIN_PDF_CUSTOMSCALE )
        {
            realwidth *= m_xscale;
            realheight *= m_yscale;
        }
        sprintf(szBuf, "[0 0 %d %d]", (int)realwidth, (int)realheight);
        mbox = szBuf;
        cObj.SetScaling(width, height);
    }

    // Precompose the object now
    cObj.SetEncrypted(IsEncrypted());
    cObj.SetImageName( actualFileName );
    cObj.PreComposeObject();

    // Now compose the contents object
/*    cObj.SetEncrypted(IsEncrypted());
    cObj.SetImageName( szBuf );
    cObj.ComposeObject();
*/
    // Now compose the page contents
    page.SetByteOffset(m_byteOffset);
    page.SetMediaBox(mbox);
    page.SetOrientation(m_Orientation);
    page.SetCurrentImageNum(m_nCurPage);

    page.SetDuplicateImage( bIsDuplicate, nObjNum );
    page.EnableThumbnailImage(bIsThumbnail);
    if ( bIsThumbnail )
    {
        page.SetDuplicateThumbnailImage(bIsDuplicateThumb, nObjNumThumb);
    }

    // Make page know the current contents object for it.
    page.SetContentsObject(&cObj);
    page.ComposeObject();

    // Maybe write the object over again if no duplicate page object
    unsigned long nPageObjNum;
    unsigned long PageCRC = page.GetCRCValue();

    bool bIsDuplicatePage = IsDuplicatePage(PageCRC, nPageObjNum);
    if ( !bIsDuplicatePage )
    {
        WriteObject(&page);
        m_pagesObj.AddObjectToKids(m_nCurObjNum);
        m_vPage.push_back(page);
        AddDuplicatePage(PageCRC, m_nCurObjNum);
        m_nCurObjNum = page.GetMaxObjectNum() + 1;
    }
    else
    {
        m_pagesObj.AddObjectToKids(nPageObjNum);
    }

    // Write the contents object
    WriteObject(&cObj);

    // Now write the Image object
    if ( !bIsDuplicate )
        WriteObject(&imgObj);

    AddDuplicateImage(imgObj.GetCRCVal(), page.GetDuplicateImageNum());
    if ( bIsThumbnail )
    {
        imgObjThumb.SetObjectNum(page.GetDuplicateThumbnailNum());
        if ( !bIsDuplicateThumb )
        {
            imgObjThumb.ComposeObject();
            WriteObject(&imgObjThumb);
        }
        AddDuplicateImage(imgObjThumb.GetCRCVal(), page.GetDuplicateThumbnailNum());
    }

    unsigned int maxFontObjectNum = GetMaxFontRefNumber();
    m_nCurObjNum = page.GetMaxObjectNum() + 1;
    m_nCurObjNum = max(maxFontObjectNum + 1, m_nCurObjNum);
    return true;
}

bool GetMaxFontRefNumInternal(const std::pair<unsigned int, PDFFont>& left,
                              const std::pair<unsigned int, PDFFont>& right)
{
    return left.second.refNum < right.second.refNum;
}

unsigned int PdfDocument::GetMaxFontRefNumber() const
{
    if ( m_mapFontNumbers.empty() )
        return 0;
    FontNumberToFontInfoMap::const_iterator it =m_mapFontNumbers.begin();
    std::advance(it, m_mapFontNumbers.size() - 1);
    return it->second.refNum;
}

bool PdfDocument::WriteObject(PDFObject* pObj)
{
    ostringstream strm;
    // Write the object number
    int nObjectNum = pObj->GetObjectNum();
    strm << nObjectNum << " 0 obj\n" << pObj->GetExtraInfo();
    string s;
    std::string strOut;
    if ( pObj->IsEncrypted() )
    {
        pObj->EncryptBlock(pObj->GetStreamContents(), strOut, nObjectNum, 0);
        s = strm.str() + strOut + pObj->GetExtraInfoEnd() + "\n" + "endobj\n";
    }
    else
        s = strm.str() + pObj->GetStreamContents() + "\n" + "endobj\n";
    m_outFile.write(s.c_str(), s.length());
    // Remember the old byte offset for this object
    ObjectInfo oi;
    oi.ObjNum = nObjectNum;
    oi.ObjOffset = m_byteOffset;
    m_vAllOffsets.push_back(oi);
    m_byteOffset += (unsigned long)s.length();
    return true;
}

bool PdfDocument::StartPDFCreation()
{
    // Open the file here
    // Every PDF needs a database
    // open the file for writing (assume OD for now)
    if ( !m_outFile )
        return false;

    m_nCurContentsObj = 4;
    m_nCurObjNum = 3;
    m_byteOffset = 0;
    m_nCurPage = 0;
    if (m_bIsAESEncrypted)
        SetPDFVersion(1,6);
    WriteHeaderInfo();

    // Write the initial catalog
    m_Catalog.SetByteOffset(m_byteOffset);
    m_Catalog.ComposeObject();
    WriteObject(&m_Catalog);

    return true;
}

bool SortByRefNum(const ObjectInfo& first, const ObjectInfo& second)
{
    return first.ObjNum < second.ObjNum;
}

void PdfDocument::SortObjects()
{
    sort(m_vAllOffsets.begin(), m_vAllOffsets.end(), SortByRefNum);
}

void PdfDocument::WriteAllFontObjects()
{
    FontNumberToFontInfoMap::iterator itCur = m_mapFontNumbers.begin();
    FontNumberToFontInfoMap::iterator itEnd = m_mapFontNumbers.end();
    FontObject fobj(-1);
    while (itCur != itEnd)
    {
        fobj.SetObjectNum(itCur->second.refNum);
        fobj.SetFontName(StringConversion::Convert_Native_To_Ansi(itCur->second.m_fontName));
        fobj.ComposeObject();
        WriteObject(&fobj);
        ++itCur;
    }
}

bool PdfDocument::IsTextElementOnPage(CTL_TEXTELEMENTNAKEDPTRLIST::const_iterator it) const
{
    unsigned int pageFlag = (*it)->displayFlags;
    bool isEvenPage = ((m_nCurPage) %2 == 0);
    switch(pageFlag & 0x0000FFFF)
    {
        case DTWAIN_PDFTEXT_ALLPAGES:
        case DTWAIN_PDFTEXT_CURRENTPAGE:
            return true;

        case DTWAIN_PDFTEXT_EVENPAGES:
            return isEvenPage;

        case DTWAIN_PDFTEXT_ODDPAGES:
            return !isEvenPage;

        case DTWAIN_PDFTEXT_FIRSTPAGE:
            return (m_nCurPage == 1);

        case DTWAIN_PDFTEXT_LASTPAGE:
            return true;
    }
    return false;
}

void PdfDocument::CreateFontNumbersFromTextElements()
{
    // Create the map of font reference numbers, given all of the text references.
    int nSize = (int)m_vPDFText.size();
    CTL_TEXTELEMENTNAKEDPTRLIST::iterator itTextElement = m_vPDFText.begin();
    int curFontNum = (int)m_mapFontNumbers.size() + 1;
    for (int i = 0; i < nSize; ++i, ++itTextElement )
    {
        // Check if text should show up on this page
        if ( !IsTextElementOnPage(itTextElement))
            continue;

        PDFTextElement* tElement = (*itTextElement);

        // Check if font is in set
        FontNameToFontInfoMap::iterator it =
            m_mapFontNames.find(tElement->m_font.m_fontName);

        if ( it == m_mapFontNames.end())
        {
            // Not found, so add it to set and to map
            PDFFont newFont(tElement->m_font.m_fontName, -1, curFontNum);
            newFont.setUsedOnPage(true);
            m_mapFontNames.insert(make_pair(tElement->m_font.m_fontName, newFont));
            m_mapFontNumbers.insert(make_pair(curFontNum, newFont));
            tElement->m_font.fontNum = curFontNum;
            ++curFontNum;
        }
        else
        {
            //            m_vPDFText[i].m_font.refNum = m_mapFontRef[it->second].refNum;
            // Get the font number from font name map
            tElement->m_font.fontNum = it->second.fontNum;
            m_mapFontNumbers[it->second.fontNum].setUsedOnPage(true);
        }
    }
//    CurFontRefNum = firstFontRef;
}

bool SortFontsByNumber(const PDFFont& left, const PDFFont& right)
{
    return left.refNum < right.refNum;
}

std::string PdfDocument::GenerateFontDictionary(int firstObjNum, int& nextObjNum)
{
    FontNumberToFontInfoMap::iterator itCur = m_mapFontNumbers.begin();
    FontNumberToFontInfoMap::iterator itEnd = m_mapFontNumbers.end();

    // Resolve the font numbers and object numbers now.
    int curObjNum = firstObjNum;
    int fontNum = 1;
    while (itCur != itEnd)
    {
        if ( itCur->second.fontNum == -1 )
        {
            itCur->second.fontNum = fontNum;
            ++fontNum;
        }
        if ( itCur->second.refNum == -1 )
        {
            itCur->second.refNum = curObjNum;
            ++curObjNum;
        }
        ++itCur;
    }

    std::string sDict;
    if ( m_mapFontNumbers.empty() )
    {
        nextObjNum = curObjNum;
        return "";
    }

    itCur = m_mapFontNumbers.begin();
    itEnd = m_mapFontNumbers.end();
/*    std::vector<PDFFont> theFonts(m_mapFontNames.size());
    int i = 0;

    // Get the list of fonts
    while (itCur != itEnd)
    {
        theFonts[i] = itCur->second;
        ++itCur;
        ++i;
    }

    // sort by number
    std::sort(theFonts.begin(), theFonts.end(), SortFontsByNumber);
    std::vector<PDFFont>::iterator itv1 = theFonts.begin();
    std::vector<PDFFont>::iterator itv2 = theFonts.end();
*/
    char szBuf[200];
    curObjNum = firstObjNum;
    sDict += "   /Font << ";
    bool bTextExists = false;
    while (itCur != itEnd)
    {
        if ( itCur->second.isUsedOnPage() )
        {
            sprintf(szBuf, "/F%d %d 0 R ", itCur->second.fontNum, itCur->second.refNum);
            sDict += szBuf;
            bTextExists = true;
        }
        itCur->second.setUsedOnPage(false); // make sure we only list this font when we need it
        ++itCur;
    }
    sDict += ">>\n";
    nextObjNum = curObjNum;
    if ( bTextExists )
        return sDict;
    return "";
}

bool PdfDocument::EndPDFCreation()
{
    // Write the Font object
    WriteAllFontObjects();

    // write the procset object
    ProcSetObject pSet(GetProcSetObjNum());
    pSet.ComposeObject();
    WriteObject(&pSet);

    // Write the pages object now
    m_pagesObj.ComposeObject();
    WriteObject(&m_pagesObj);
    m_nCurObjNum += (int)m_mapFontRef.size();
    int nEncryptObjectNum = m_nCurObjNum;
    if ( IsEncrypted() )
    {
        // Write the encryption object
        EncryptionObject EObject(m_nCurObjNum);
        EObject.SetAESEncryption(m_bIsAESEncrypted);
        EObject.AssignParent(this);
        EObject.SetFilter("Standard");
        EObject.SetLength(m_bIsStrongEncryption?128:40);
        EObject.SetOwnerPassword(m_EncryptionPassword[OWNER_PASSWORD]);
        EObject.SetUserPassword(m_EncryptionPassword[USER_PASSWORD]);
        if (m_bIsAESEncrypted)
        {
            EObject.SetRValue(4);
            EObject.SetVValue(4);
        }
        else
        if ( m_bIsStrongEncryption )
        {
            EObject.SetRValue(3);
            EObject.SetVValue(2);
        }
        else
        {
            EObject.SetRValue(2);
            EObject.SetVValue(1);
        }
        EObject.ComposeObject();
        WriteObject(&EObject);
        m_nCurObjNum++;
    }

    InfoObject Info(m_nCurObjNum);
    Info.SetEncrypted(IsEncrypted());
    Info.AssignParent(this);
    Info.SetAuthor(m_sAuthor);
    Info.SetKeywords( m_sKeywords );
    Info.SetProducer( m_sProducer );
    Info.SetSubject( m_sSubject );
    Info.SetTitle( m_sTitle );
    Info.SetCreator(m_sCreator);
    Info.ComposeObject();

    WriteObject(&Info);

       // will do xref stuff here
    char szBuf[50];
    ostringstream strmXRef;
    size_t nObjects = m_vAllOffsets.size();
    size_t nTotalObjects = nObjects + 1;
    strmXRef << "xref\n0 " << nTotalObjects << "\n";
    strmXRef << "0000000000 65535 f " << "\n";

    SortObjects();
    for ( size_t i = 0; i < nObjects; ++i)
    {
        sprintf(szBuf,"%010d", static_cast<int>(m_vAllOffsets[i].ObjOffset));
        strmXRef << szBuf << " 00000 n \n";
    }
    strmXRef << "trailer\n<</Size " << nObjects + 1 << "\n/Root 1 0 R\n";
    strmXRef << "/Info " << m_nCurObjNum << " 0 R\n";
    strmXRef << "/ID" << "[<" << m_DocumentID[0] << "> <" << m_DocumentID[1] << ">]\n";
    if ( IsEncrypted())
        strmXRef << "/Encrypt " << nEncryptObjectNum << " 0 R\n";
    strmXRef << ">>\n";
    strmXRef << "\nstartxref\n" << m_byteOffset << "\n%%EOF";
    string sXref = strmXRef.str();

    m_outFile << sXref;

    if ( m_outFile)
    {
        m_outFile.close();
    }
    return true;
}

void PdfDocument::SetMediaBox(int mediatype)
{
    if ( mediatype == -1)
    {
        m_bUseVariableMediaBox = true;
        return;
    }
    MediaBoxMap::iterator it = m_mediaMap.find(mediatype);
    if ( it != m_mediaMap.end())
        SetMediaBox((*it).second.second);
    m_bUseVariableMediaBox = false;
}

void PagesObject::ComposeObject()
{
    SetContents("<< /Type /Pages\n   /Kids [");
    size_t nSize = KidsArrayObjects.size();
    char szBuf[100];
    for ( size_t i = 0; i < nSize; i++ )
    {
        sprintf(szBuf,"%d 0 R ", KidsArrayObjects[i]);
        AppendContents(szBuf);
        if ( (i % 10 == 0) && i > 0 )
            AppendContents("\n          ");
    }
    AppendContents("]\n");
    AppendContents("   /Count ");
    sprintf(szBuf,"%d\n >>", static_cast<int>(KidsArrayObjects.size()));
    AppendContents(szBuf);
}


void InfoObject::ComposeObject()
{
    m_sDate = GetPDFDate();
    PdfDocument *pParent = GetParent();
    if ( !pParent || !pParent->IsEncrypted())
    {
        SetContents("<<\n"
                    "/Creator  (" + MakeCompatiblePDFString(m_sCreator.substr(1, m_sCreator.length() - 2)) + ")\n"
                    "/Producer (" + MakeCompatiblePDFString(m_sProducer.substr(1, m_sProducer.length() - 2)) + ")\n"
                    "/Author   (" + MakeCompatiblePDFString(m_sAuthor.substr(1, m_sAuthor.length() - 2)) + ")\n"
                    "/Title    (" + MakeCompatiblePDFString(m_sTitle.substr(1, m_sTitle.length() - 2)) + ")\n"
                    "/Subject  (" + MakeCompatiblePDFString(m_sSubject.substr(1, m_sSubject.length() - 2)) + ")\n"
                    "/Keywords (" + MakeCompatiblePDFString(m_sKeywords.substr(1, m_sKeywords.length() - 2)) + ")\n"
                    "/CreationDate (" + m_sDate + ")\n"
                    ">>");
    }
    else
    {
        typedef std::pair<std::string, std::string> EncryptedDataPair;
        std::vector< EncryptedDataPair > AllEncryptedData( 7 );
        AllEncryptedData[0].first = m_sCreator.substr(1, m_sCreator.length() - 2);
        AllEncryptedData[1].first = m_sProducer.substr(1, m_sProducer.length() - 2);
        AllEncryptedData[2].first = m_sAuthor.substr(1, m_sAuthor.length() - 2);
        AllEncryptedData[3].first = m_sTitle.substr(1, m_sTitle.length() - 2);
        AllEncryptedData[4].first = m_sSubject.substr(1, m_sSubject.length() - 2);
        AllEncryptedData[5].first = m_sKeywords.substr(1, m_sKeywords.length() - 2);
        AllEncryptedData[6].first = m_sDate = m_sDate;

        for ( int i = 0; i < 7; ++i )
        {
            EncryptBlock(AllEncryptedData[i].first, AllEncryptedData[i].second, GetObjectNum(), 0);
        }

        SetContents("<<\n"
                    "/Creator  (" + MakeCompatiblePDFString(AllEncryptedData[0].second) + ")\n"
                    "/Producer (" + MakeCompatiblePDFString(AllEncryptedData[1].second) + ")\n"
                    "/Author   (" + MakeCompatiblePDFString(AllEncryptedData[2].second) + ")\n"
                    "/Title    (" + MakeCompatiblePDFString(AllEncryptedData[3].second) + ")\n"
                    "/Subject  (" + MakeCompatiblePDFString(AllEncryptedData[4].second) + ")\n"
                    "/Keywords (" + MakeCompatiblePDFString(AllEncryptedData[5].second) + ")\n"
                    "/CreationDate ("+ MakeCompatiblePDFString(AllEncryptedData[6].second) + ")\n"
                    ">>");
    }
    SetEncrypted(false); // The encryption is done internally, so no need to do it later
}


void EncryptionObject::ComposeObject()
{
    char szBuf[100];
    SetContents("<<\n");
    // R Value, Length;
    if (m_bAESEncrypted)
    {
        sprintf(szBuf, "<</CF<</StdCF<</AuthEvent/DocOpen/CFM/AESV2/Length 16>>>>");
        AppendContents(szBuf);
    }
    if ( m_RValue == 3 )
        sprintf(szBuf,"/R %d\n/Length %d\n", m_RValue, m_nLength);
    else
        sprintf(szBuf,"/R %d\n", m_RValue);
    AppendContents(szBuf);
    AppendContents("/Filter /Standard\n");

    // Now for the owner and user passwords
    std::string enc1;
    std::string enc2;
    PDFEncryption::UCHARArray enc1Array = GetParent()->GetEncryptionEngine().GetOwnerKey();
    PDFEncryption::UCHARArray enc2Array = GetParent()->GetEncryptionEngine().GetUserKey();
    enc1.append(reinterpret_cast<const char *>(enc1Array.data()), 32);
    enc2.append(reinterpret_cast<const char *>(enc2Array.data()), 32);
    AppendContents("/O (");
    enc1 = MakeCompatiblePDFString(enc1);
    WriteRaw(enc1.data(), enc1.length());
    AppendContents(")\n/U (");
    enc2 = MakeCompatiblePDFString(enc2);
    WriteRaw(enc2.data(), enc2.length());
    sprintf(szBuf,")\n/P %d\n/V %d\n>>", GetParent()->GetEncryptionEngine().GetPermissions(), m_nVValue);
    AppendContents(szBuf);
    if (m_bAESEncrypted)
    {
        sprintf(szBuf, "/StmF /StdCF /StrF /StdCF");
        AppendContents(szBuf);
    }
}

bool IsRenderModeStroked(int rendermode)
{
    return (rendermode == 1 ||
        rendermode == 2 ||
        rendermode == 5 ||
        rendermode == 6);
}

std::string PDFTextElement::GetPDFTextString() const
{
    char szBuf[100];
    std::string sText;

    // Get the font
    sprintf(szBuf, "\n/F%d %4.2lf Tf", m_font.fontNum, fontSize);
    sText += szBuf;

    // Get the color
    double red = (double)GetRValue(colorRGB) / 255.0;
    double green = (double)GetGValue(colorRGB) / 255.0;
    double blue = (double)GetBValue(colorRGB) / 255.0;
    sprintf(szBuf, "\n%4.2lf %4.2lf %4.2lf rg", red, green, blue);
    sText += szBuf;

    // Get the position
    sprintf(szBuf, "\n1 0 0 1 %lf %lf Tm\n", xpos, ypos);

//    sprintf(szBuf, "\n%d %d Td ", xpos, ypos);
    sText += szBuf;

    // Get the character spacing
    sprintf(szBuf, "\n%4.2lf Tc ", charSpacing);
    sText += szBuf;

    // Get the word spacing
    sprintf(szBuf, "\n%4.2lf Tw ", wordSpacing);
    sText += szBuf;

    // Get the scaling
    sprintf(szBuf, "\n%4.2lf Tz ", scaling);
    sText += szBuf;

    // Set the render mode
    sprintf(szBuf, "\n%d Tr ", renderMode);
    sText += szBuf;

    // Get the leading
    sprintf(szBuf, "\n8.8 TL\n");
    sText += szBuf;

    // Get the stroke width
    if ( IsRenderModeStroked(renderMode))
    {
        sprintf(szBuf, "\n%d w ", strokeWidth);
        sText += szBuf;
    }

    sText += "\n(" + MakeCompatiblePDFString(StringConversion::Convert_Native_To_Ansi(m_text)) + ")Tj\n";

    // Reset the position for next text item
  //  sText += "1 0 0 1 0 0 Tm\n";
    return sText;
}

void ContentsObject::PreComposeObject()
{
    string sLength;
    string sStream;
    string sRealStream;
    char szBuf[120];
    sLength = "/Length ";
    sStream = "stream\n";

    // Start of stream
    sRealStream += "q\n";
    sRealStream += "1.0000 0.0000 0.0000 1.0000 0.0000 0.0000 cm\n";
    sprintf(szBuf, "%-10.5lf 0.0000 0.0000 %10.5lf 0.000 0.0000 cm\n", m_xscale, m_yscale);
    sRealStream += szBuf;
    sRealStream += "/" + m_sImgName + " Do\nQ\n";
    m_preComposedObject = sRealStream;
}

void ContentsObject::CreateFontDictAndText(int startObjNum, int& nextObjNum)
{
    typedef std::pair<int, double> FontPairKey;
    typedef std::unordered_map<FontPairKey, std::vector<PDFTextElement*>, boost::hash<FontPairKey>> FontToElementMap;

    FontToElementMap fontToElementMap;

    // now resolve the font numbers
    PdfDocument *pParent = GetParent();
    pParent->CreateFontNumbersFromTextElements();
    int tempNum;
    m_sFontDictString = pParent->GenerateFontDictionary(startObjNum, tempNum);
    nextObjNum = tempNum;

    std::string m_sText;
    m_sFontString = "";

    int numTextElements = pParent->GetNumTextElements();
    bool printOnPage = false;
    int numPrinted = 0;
    bool onEvenPage = (pParent->GetCurrentPageNumber() % 2) == 0;
    CTL_TEXTELEMENTNAKEDPTRLIST::const_iterator it = pParent->GetFirstTextElement();
    for ( int nElements = 0; nElements < numTextElements; ++nElements, ++it )
    {
        printOnPage = false;
        unsigned int pageFlag = (*it)->displayFlags;
        if ( pageFlag & DTWAIN_PDFTEXT_DISABLED )
            continue; // ignore -- text element is not displayed on page

        if ( pageFlag & (DTWAIN_PDFTEXT_ALLPAGES | DTWAIN_PDFTEXT_CURRENTPAGE ))
        {
            printOnPage = true;
            if ((pageFlag & DTWAIN_PDFTEXT_CURRENTPAGE) && (*it)->hasBeenDisplayed)
                printOnPage = false;
        }
        else
        if ( (pageFlag & DTWAIN_PDFTEXT_EVENPAGES) && onEvenPage )
            printOnPage = true;
        else
        if ( (pageFlag & DTWAIN_PDFTEXT_ODDPAGES) && !onEvenPage )
            printOnPage = true;
        else
        if  ((pageFlag & DTWAIN_PDFTEXT_FIRSTPAGE) && pParent->GetCurrentPageNumber() == 1 )
            printOnPage = true;
        if ( printOnPage )
        {
            // Add this to font to PDF map
            PDFTextElement* elm = *it;
            elm->hasBeenDisplayed = true;
            FontPairKey fpk(elm->m_font.fontNum, elm->fontSize);
            FontToElementMap::iterator it2 = fontToElementMap.find(fpk);
            if ( it2 == fontToElementMap.end() )
                it2 = fontToElementMap.insert(make_pair(fpk, std::vector<PDFTextElement*>())).first;
            it2->second.push_back(elm);
            ++numPrinted;
        }
    }

    if ( numPrinted > 0 )
    {
        char szBuf[100];

        // go through map and print
        FontToElementMap::iterator it1 = fontToElementMap.begin();
        FontToElementMap::iterator it2 = fontToElementMap.end();
        Matrix3_3 pdfTransformation;
        Matrix3_3 pdfTransformation2;
        Matrix3_3 pdfTransformation3;
        Matrix3_3 MatrixTranslate;
        Matrix3_3 MatrixScale;
        Matrix3_3 MatrixRotate;
        Matrix3_3 MatrixSkew;
        const Matrix3_3* allTransformations[24][4] =
        { {&MatrixTranslate, &MatrixScale, &MatrixRotate, &MatrixSkew}, // TSRK
          {&MatrixTranslate, &MatrixScale, &MatrixSkew, &MatrixRotate}, // TSKR
          {&MatrixTranslate, &MatrixSkew, &MatrixScale, &MatrixRotate}, // TKSR
          {&MatrixTranslate, &MatrixSkew, &MatrixRotate, &MatrixScale}, // TKRS
          {&MatrixTranslate, &MatrixRotate, &MatrixScale, &MatrixSkew}, // TRSK
          {&MatrixTranslate, &MatrixRotate, &MatrixSkew, &MatrixScale}, // TRKS

          {&MatrixScale, &MatrixTranslate, &MatrixRotate, &MatrixSkew}, // STRK
          {&MatrixScale, &MatrixTranslate, &MatrixSkew, &MatrixRotate}, // STKR
          {&MatrixScale, &MatrixSkew, &MatrixTranslate, &MatrixRotate}, // SKTR
          {&MatrixScale, &MatrixSkew, &MatrixRotate, &MatrixTranslate}, // SKRT
          {&MatrixScale, &MatrixRotate, &MatrixTranslate, &MatrixSkew}, // SRTK
          {&MatrixScale, &MatrixRotate, &MatrixSkew, &MatrixTranslate}, // SRKT

          {&MatrixRotate, &MatrixScale, &MatrixTranslate, &MatrixSkew}, // RSTK
          {&MatrixRotate, &MatrixScale, &MatrixSkew, &MatrixTranslate}, // RSKT
          {&MatrixRotate, &MatrixTranslate, &MatrixScale, &MatrixSkew}, // RTSK
          {&MatrixRotate, &MatrixTranslate, &MatrixSkew, &MatrixScale}, // RTKT
          {&MatrixRotate, &MatrixSkew, &MatrixScale, &MatrixTranslate}, // RKST
          {&MatrixRotate, &MatrixSkew, &MatrixTranslate, &MatrixScale}, // RKTS

          {&MatrixSkew, &MatrixScale, &MatrixTranslate, &MatrixRotate}, // KSTR
          {&MatrixSkew, &MatrixScale, &MatrixRotate, &MatrixTranslate}, // KSRT
          {&MatrixSkew, &MatrixRotate, &MatrixScale, &MatrixTranslate}, // KRST
          {&MatrixSkew, &MatrixRotate, &MatrixTranslate, &MatrixScale}, // KRTS
          {&MatrixSkew, &MatrixTranslate, &MatrixScale, &MatrixRotate}, // KTSR
          {&MatrixSkew, &MatrixTranslate, &MatrixRotate, &MatrixScale}  // KTRS
        };

        std::vector<const Matrix3_3*> MatrixAll(4);
        while (it1 != it2 )
        {
            // Get the font
            sprintf(szBuf, "\n/F%d %4.2lf Tf", it1->first.first, it1->first.second);
            m_sText += szBuf;
            std::vector<PDFTextElement*>::iterator pIt1 = it1->second.begin();
            std::vector<PDFTextElement*>::iterator pIt2 = it1->second.end();

            MatrixTranslate[0][2] = 0;
            MatrixTranslate[1][2] = 0;
            MatrixTranslate[2][2] = 1;
            MatrixScale[0][2] = 0;
            MatrixScale[1][2] = 0;
            MatrixScale[2][2] = 1;
            MatrixRotate[0][2] = 0;
            MatrixRotate[1][2] = 0;
            MatrixRotate[2][2] = 1;
            MatrixSkew[0][2] = 0;
            MatrixSkew[1][2] = 0;
            MatrixSkew[2][2] = 1;

            /*std::map<char, Matrix3_3*> TransformationMap;
            TransformationMap['T'] = &MatrixTranslate;
            TransformationMap['S'] = &MatrixScale;
            TransformationMap['R'] = &MatrixRotate;
            TransformationMap['K'] = &MatrixSkew;*/

            while (pIt1 != pIt2)
            {
                // Get the color
                double red = (double)GetRValue((*pIt1)->colorRGB) / 255.0;
                double green = (double)GetGValue((*pIt1)->colorRGB) / 255.0;
                double blue = (double)GetBValue((*pIt1)->colorRGB) / 255.0;
                sprintf(szBuf, "\n%4.2lf %4.2lf %4.2lf rg", red, green, blue);
                m_sText += szBuf;

                // Test matrix multiplication of all components
                // Try translate x rotate x scale/skew
                MatrixTranslate[0][0] = 1;
                MatrixTranslate[0][1] = 0;
                MatrixTranslate[1][0] = 0;
                MatrixTranslate[1][1] = 1;
                MatrixTranslate[2][0] = (*pIt1)->xpos;
                MatrixTranslate[2][1] = (*pIt1)->ypos;

                MatrixScale[0][0] = (*pIt1)->scalingX;
                MatrixScale[0][1] = 0;
                MatrixScale[1][0] = 0;
                MatrixScale[1][1] = (*pIt1)->scalingY;
                MatrixScale[2][0] = 0;
                MatrixScale[2][1] = 0;

                MatrixRotate[0][0] = cos(DegreesToRadians((*pIt1)->rotationAngle));
                MatrixRotate[0][1] = sin(DegreesToRadians((*pIt1)->rotationAngle));
                MatrixRotate[1][0] = -sin(DegreesToRadians((*pIt1)->rotationAngle));
                MatrixRotate[1][1] = cos(DegreesToRadians((*pIt1)->rotationAngle));
                MatrixRotate[2][0] = 0;
                MatrixRotate[2][1] = 0;

                MatrixSkew[0][0] = 1;
                MatrixSkew[0][1] = tan(DegreesToRadians((*pIt1)->skewAngleX));
                MatrixSkew[1][0] = tan(DegreesToRadians((*pIt1)->skewAngleY));
                MatrixSkew[1][1] = 1;
                MatrixSkew[2][0] = 0;
                MatrixSkew[2][1] = 0;

                for (int i = 0; i < 4; ++i )
                    MatrixAll[i] = allTransformations[(*pIt1)->textTransform][i];

               pdfTransformation = MultiplyMatrix33(*MatrixAll[1], *MatrixAll[0]);

                // Do rotation transformation
                pdfTransformation2 = MultiplyMatrix33(*MatrixAll[2], pdfTransformation);

                // Do skew
                pdfTransformation3 = MultiplyMatrix33(*MatrixAll[3], pdfTransformation2);

                sprintf(szBuf, "\n%4.2lf %4.2lf %4.2lf %4.2lf %4.2lf %4.2lf Tm",
                    pdfTransformation3[0][0], pdfTransformation3[0][1], pdfTransformation3[1][0], pdfTransformation3[1][1],
                    pdfTransformation3[2][0], pdfTransformation3[2][1]); //pIt1->scalingX, pIt1->scalingY, pIt1->xpos, pIt1->ypos);

                m_sText += szBuf;

                // get the rotation
/*                if ( !FLOAT_CLOSE(pIt1->rotationAngle,0.0))
                {
                    sprintf(szBuf, "\n%4.2lf %4.2lf -%4.2lf %4.2lf 0 0 Tm", pIt1->rotationAngle,
                                    pIt1->rotationAngle, pIt1->rotationAngle, pIt1->rotationAngle);
                    m_sText += szBuf;
                }
*/
                // get the skew if specified
/*                if ( !FLOAT_CLOSE(pIt1->skewAngleX,0.0) || !FLOAT_CLOSE(pIt1->skewAngleY, 0.0))
                {
                    sprintf(szBuf, "\n1 %4.2lf %4.2lf 1 0 0 Tm", pIt1->skewAngleX, pIt1->skewAngleY);
                    m_sText += szBuf;
                }
*/
                // Get the character spacing
                sprintf(szBuf, "\n%4.2lf Tc ", (*pIt1)->charSpacing);
                m_sText += szBuf;

                // Get the word spacing
                sprintf(szBuf, "\n%4.2lf Tw ", (*pIt1)->wordSpacing);
                m_sText += szBuf;

                // Get the scaling
                sprintf(szBuf, "\n%4.2lf Tz ", (*pIt1)->scaling);
                m_sText += szBuf;

                // Set the render mode
                sprintf(szBuf, "\n%d Tr ", (*pIt1)->renderMode);
                m_sText += szBuf;

				// Get the position
				sprintf(szBuf, "\n1 0 0 1 %lf %lf Tm\n", (*pIt1)->xpos, (*pIt1)->ypos);
                m_sText += szBuf;

                // Get the leading
            /*  sprintf(szBuf, "\n8.8 TL");
                m_sText += szBuf;*/

                // Get the stroke width
                if ( IsRenderModeStroked((*pIt1)->renderMode))
                {
                    sprintf(szBuf, "\n%d w ", (*pIt1)->strokeWidth);
                    m_sText += szBuf;
                }

                m_sText += "\n(" + MakeCompatiblePDFString(StringConversion::Convert_Native_To_Ansi((*pIt1)->m_text)) + ") Tj";
                ++pIt1;
            }
            ++it1;
        }
        m_sFontString = "BT" + m_sText + "\nET\n";
    }

    // Now it should be safe to remove the text that is
    // only to print on the current page
    pParent->RemoveTempTextElements();
}

void ContentsObject::ComposeObject()
{
    string sLength;
    string sStream;
    string sRealStream;
    char szBuf[120];
    size_t nLength;

    sLength = "/Length ";
    sStream = "stream\n";

    // Start of stream

    // Get the stream string that has already been composed
    sRealStream = m_preComposedObject + m_sFontString;

    nLength = sRealStream.length();
    // end of graphics stream

    // Compress stream?
    vector<char> VecIn(sRealStream.begin(), sRealStream.end());
    vector<char> VecOut;

    // attempt to compress twice (as a test)
    // first compress with flate, then a85
    PdfDocument::CompressTypes compresstype[] = {PdfDocument::NO_COMPRESS,PdfDocument::FLATE_COMPRESS,PdfDocument::A85_COMPRESS};
    const char *fstrings[] = {"", " /FlateDecode ", " /ASCII85Decode " };

    std::string filterstr;

    for ( size_t i = 0; i < sizeof(compresstype) / sizeof(compresstype[0]); ++i )
    {
        if ( compresstype[i] == PdfDocument::NO_COMPRESS)
        {
            if ( GetParent()->IsNoCompression() )
                break;
            else
                continue;
        }
        else
        if ( compresstype[i] == PdfDocument::A85_COMPRESS)
        {
            if ( GetParent()->IsASCIICompressed() )
                EncodeVectorStream(VecIn, VecIn.size(), VecOut, compresstype[i]);
            else
                continue;
        }
        else
            EncodeVectorStream(VecIn, VecIn.size(), VecOut, compresstype[i]);
        if ( !GetParent()->IsNoCompression() )
        {
            VecIn = VecOut;
            VecIn.swap(VecIn);
            nLength = VecIn.size();
            filterstr = fstrings[i] + filterstr;
        }
        else
        if ( VecOut.size() > VecIn.size() &&
             compresstype[i] == PdfDocument::FLATE_COMPRESS )

        {
            // Compression failed, so no filtering is to be done
        }

    }

    if ( !GetParent()->IsNoCompression() )
        filterstr = "[" + filterstr + "]";

     // Encrypt this block of data if encryption is set
    const char *pStreamToWrite;
	pStreamToWrite = VecIn.data();

    AppendContents("<< ");

    if ( !GetParent()->IsNoCompression() )
        AppendContents( "/Filter " + filterstr);

    // Encrypt this block of data if encryption is set
    sprintf(szBuf, "%d >>\n", static_cast<int>(nLength));
    AppendContents(sLength + szBuf + sStream);

    if ( IsEncrypted() )
    {
        m_sExtraInfo = GetContents();
        m_sContents.assign(pStreamToWrite, nLength);
        m_sExtraInfoEnd = "\nendstream\n";
    }
    else
    {
        WriteRaw(pStreamToWrite, nLength);
        AppendContents("\nendstream\n");
    }
}

std::string ContentsObject::GetExtraInfo()
{
    return m_sExtraInfo;
}

std::string ContentsObject::GetExtraInfoEnd()
{
    return m_sExtraInfoEnd;
}

std::string ContentsObject::GetStreamContents()
{
    if ( IsEncrypted() )
        return m_sContents;
    return GetContents();
}

bool ImageObject::OpenAndComposeImage(int& width, int& height, int& bpp, int& rgb, int& dpix,
                                      int& dpiy)
{
    vector<char> t;
    m_vImgStream = t;  // This resizes to 0

    bool bRet = false;
    m_nImgType = GetParent()->GetImageType();
    if ( m_nImgType == 0 )
    {
        bRet = ProcessJPEGImage(width, height, bpp, rgb);
        dpix = GetParent()->GetDPI();
        dpiy = GetParent()->GetDPI();
        dpix = max(dpix, 1);
        dpiy = max(dpiy, 1);
    }
    else
        bRet = ProcessBMPImage(width, height, bpp, rgb, dpix, dpiy);
    return bRet;
}


bool ImageObject::ProcessJPEGImage(int& width, int& height, int& bpp, int& rgb)
{
    struct jpeg_decompress_struct cinfo;
    struct jpeg_error_mgr jerr;
    FILE *infile;

    // First open the image file (this will always be a JPEG file
    // Open the file -- why is this a memory leak?
    if ((infile = fopen(StringConversion::Convert_Native_To_Ansi(m_sImgName).c_str(), "rb")) == NULL)
        return false;
    {
        // Setup the decompression options
        cinfo.err = jpeg_std_error (&jerr);

        // Start decompressing
        jpeg_create_decompress (&cinfo);
        jpeg_stdio_src (&cinfo, infile);
        jpeg_read_header (&cinfo, TRUE);

        bpp = m_bpp = cinfo.data_precision;

        switch ( cinfo.jpeg_color_space)
        {
        case JCS_GRAYSCALE:
                m_sColorSpace = "DeviceGray";
                rgb = false;
            break;

            default:
                m_sColorSpace = "DeviceRGB";
                rgb = true;
            break;
        }

        width = m_Width = cinfo.image_width;
        height = m_Height = cinfo.image_height;

      // This cleans things up for us in the JPEG library
        jpeg_destroy_decompress (&cinfo);
        fclose (infile);
    }
    /****************************************************************************
    Read the image into it's memory buffer
    ****************************************************************************/
    m_imgLengthInBytes = 0;
    size_t nMaxRead = 50000;
    m_vImgStream.resize(nMaxRead);

    if ((infile = fopen (StringConversion::Convert_Native_To_Ansi(m_sImgName).c_str(), "rb")) == NULL)
        return false;
    size_t nCount;

    int nLoops = 1;
    while ( true )
    {
        nCount = fread(&m_vImgStream[m_imgLengthInBytes], sizeof(char), nMaxRead, infile);

        if ( nCount == 0  )
            break;

        m_imgLengthInBytes += nCount;

        // Leave if we've read everything
        if ( nCount < nMaxRead )
            break;

        // Need to resize the array
        nLoops++;
        m_vImgStream.resize(nMaxRead * nLoops);
    }
    fclose (infile);

    unsigned long crcVal = crc32_aux(0, (unsigned char *)m_vImgStream.data(), (unsigned int)m_imgLengthInBytes);

    m_nCurCRCVal = crcVal;
    return true;
}


bool ImageObject::ProcessTIFFImage(int& width, int& height, int& bpp, int& /*rgb*/, int& dpix, int& dpiy)
{
  /**************************************************************************
    Some notes about TIFF support inside PDF files.

     - MSB2LSB is the only byte fillorder that is supported.
     - The images must be converted to single strip
     - G3 and G4 are supported
  **************************************************************************/

    TIFF *image, *conv = 0;
    int stripCount, stripMax;
    tsize_t stripSize;
    unsigned long imageOffset;
    vector<char> stripBuffer;
    uint16 tiffResponse16, compression, fillorder;

    uint32 theight, twidth;

  // Open the file and make sure that it exists and is a TIFF file
    if ((image = TIFFOpen(StringConversion::Convert_Native_To_Ansi(m_sImgName).c_str(), "r")) == NULL)
    {
        return false;
    }


    // Bits per component is per colour component, not per sample. Does this
    // matter?
    if (TIFFGetField (image, TIFFTAG_BITSPERSAMPLE, &tiffResponse16) != 0)
      m_bpp = bpp = tiffResponse16;

    // The colour device will change based on the number of samples per pixel
    if (TIFFGetField (image, TIFFTAG_SAMPLESPERPIXEL, &tiffResponse16) == 0)
    {
        return false;
    }

    switch (tiffResponse16)
    {
        case 1:
            m_sColorSpace = "DeviceGray";
        break;

        default:
            m_sColorSpace = "DeviceRGB";
        break;
    }

    /****************************************************************************
    We need to add a sub-dictionary with the parameters for the compression
    filter in it.
    ****************************************************************************/

    // K will be minus one for g4 fax, and zero for g3 fax
    TIFFGetField (image, TIFFTAG_COMPRESSION, &compression);
    switch (compression)
    {
        case COMPRESSION_CCITTFAX3:
            m_nTiffKValue = 0;
        break;

        case COMPRESSION_CCITTFAX4:
            m_nTiffKValue = -1;
        break;

    }

    // Get the width and height of the image
    TIFFGetField (image, TIFFTAG_IMAGEWIDTH, &width);
    TIFFGetField (image, TIFFTAG_IMAGELENGTH, &height);

    // Get the resolution of the image
    float xres;
    float yres;

    TIFFGetField (image, TIFFTAG_XRESOLUTION, &xres);
    TIFFGetField (image, TIFFTAG_YRESOLUTION, &yres);

    dpix = (int)xres;
    dpiy = (int)yres;

    m_Width = m_nTiffColumns = width;
    m_Height = m_nTiffRows = height;

  // Fillorder determines whether we convert on the fly or not, although
  // multistrip images also need to be converted
    TIFFGetField (image, TIFFTAG_FILLORDER, &fillorder);

    if ( (fillorder == FILLORDER_LSB2MSB) || (TIFFNumberOfStrips (image) > 1))
    {
    /*************************************************************************
      Convert the image
    *************************************************************************/

      m_vImgStream.resize(0);
      m_sTiffOffset = 0;

      // Open the dummy document (which actually only exists in memory)
      conv =
        TIFFClientOpen ("dummy", "w", (thandle_t) - 1,
            libtiffReadProc,
            libtiffWriteProc,
            libtiffSeekProc,
            libtiffCloseProc,
            libtiffSizeProc,
            NULL,
            NULL);

      // Copy the image information ready for conversion
      stripSize = TIFFStripSize (image);
      stripMax = TIFFNumberOfStrips (image);
      imageOffset = 0;

      stripBuffer.resize(TIFFNumberOfStrips (image) * stripSize);

      for (stripCount = 0; stripCount < stripMax; stripCount++)
      {
         imageOffset += static_cast<unsigned long>(TIFFReadEncodedStrip (image, stripCount,
                                                    &stripBuffer[imageOffset],
                                                static_cast<tmsize_t>(stripSize)));
      }

      // We also need to copy some of the attributes of the tiff image
      // Bits per sample has to be 1 because this is going to be a G4/G3 image
      // (and all other image formats were stripped out above).
      twidth = width;

      theight = height;
      TIFFSetField (conv, TIFFTAG_IMAGEWIDTH, twidth);
      TIFFSetField (conv, TIFFTAG_IMAGELENGTH, theight);
      TIFFSetField (conv, TIFFTAG_BITSPERSAMPLE, 1);
      TIFFSetField (conv, TIFFTAG_COMPRESSION, compression);
      TIFFSetField (conv, TIFFTAG_PLANARCONFIG, PLANARCONFIG_CONTIG);
      TIFFSetField (conv, TIFFTAG_ROWSPERSTRIP, height + 1);
//      TIFFSetField (conv, TIFFTAG_PHOTOMETRIC, PHOTOMETRIC_MINISWHITE);
      TIFFSetField (conv, TIFFTAG_PHOTOMETRIC, PHOTOMETRIC_MINISBLACK);
      TIFFSetField (conv, TIFFTAG_FILLORDER, FILLORDER_MSB2LSB);
      TIFFSetField (conv, TIFFTAG_SAMPLESPERPIXEL, 1);
      TIFFSetField (conv, TIFFTAG_ORIENTATION, ORIENTATION_TOPLEFT);
      TIFFSetField (conv, TIFFTAG_RESOLUTIONUNIT, RESUNIT_INCH);
      TIFFSetField (conv, TIFFTAG_XRESOLUTION, 300);
      TIFFSetField (conv, TIFFTAG_YRESOLUTION, 300);
      TIFFSetField (conv, TIFFTAG_SOFTWARE, "DynaRithmic TWAIN Library (DTWAIN) 5.0");

      if (compression == COMPRESSION_CCITTFAX4)
            TIFFSetField (conv, TIFFTAG_GROUP4OPTIONS, 0);

      // Actually do the conversion
      TIFFWriteEncodedStrip (conv, 0, stripBuffer.data(), imageOffset);

      // Finish up
      m_imgLengthInBytes = m_sTiffOffset;
    }

  else
    {
    /**************************************************************************
       Insert the image
    **************************************************************************/
      // We also need to add a binary stream to the object and put the image
      // data into this stream
      stripSize = TIFFStripSize (image);
      imageOffset = 0;

      int nStrips = TIFFNumberOfStrips(image);
      m_vImgStream.resize( nStrips * stripSize );

      for (stripCount = 0; stripCount < nStrips;  stripCount++)
      {
         imageOffset += static_cast<unsigned long>(TIFFReadRawStrip (image, stripCount,
                            &m_vImgStream[imageOffset], stripSize));
      }

      // We might have too much memory, truncate to the size we have actually
      // read
      m_vImgStream.resize(imageOffset);
      m_imgLengthInBytes = imageOffset;
    }

    unsigned long crcVal = crc32_aux(0, (unsigned char *)m_vImgStream.data(), (unsigned int)m_imgLengthInBytes);

    m_nCurCRCVal = crcVal;
    TIFFClose (image);
    TIFFClose (conv);
    return true;
}


bool ImageObject::ProcessBMPImage(int& width, int& height, int& bpp, int& /*rgb*/, int& dpix, int& dpiy)
{
    TIFF *image, *conv = 0;
    int stripCount, stripMax;
    tsize_t stripSize;
    unsigned long imageOffset;
    vector<char> stripBuffer;
    uint16 tiffResponse16, compression, fillorder = 0;

    uint32 theight, twidth;

    // Open the file and make sure that it exists and is a TIFF file
    if ((image = TIFFOpen (StringConversion::Convert_Native_To_Ansi(m_sImgName).c_str(), "r")) == NULL)
    {
        return false;
    }


    // Bits per component is per colour component, not per sample. Does this
    // matter?
    if (TIFFGetField (image, TIFFTAG_BITSPERSAMPLE, &tiffResponse16) != 0)
      m_bpp = bpp = tiffResponse16;

    // The colour device will change based on the number of samples per pixel
    if (TIFFGetField (image, TIFFTAG_SAMPLESPERPIXEL, &tiffResponse16) == 0)
    {
        return false;
    }

    switch (tiffResponse16)
    {
        case 1:
            m_sColorSpace = "DeviceGray";
        break;

        default:
            m_sColorSpace = "DeviceRGB";
        break;
    }

    /****************************************************************************
    We need to add a sub-dictionary with the parameters for the compression
    filter in it.
    ****************************************************************************/

    // K will be minus one for g4 fax, and zero for g3 fax
    TIFFGetField (image, TIFFTAG_COMPRESSION, &compression);
    switch (compression)
    {
        case COMPRESSION_CCITTFAX3:
            m_nTiffKValue = 0;
        break;

        case COMPRESSION_CCITTFAX4:
            m_nTiffKValue = -1;
        break;

    }

    // Get the width and height of the image
    TIFFGetField (image, TIFFTAG_IMAGEWIDTH, &width);
    TIFFGetField (image, TIFFTAG_IMAGELENGTH, &height);

    // Get the resoulution of the image
    float xres;
    float yres;

    TIFFGetField (image, TIFFTAG_XRESOLUTION, &xres);
    TIFFGetField (image, TIFFTAG_YRESOLUTION, &yres);

    dpix = (int)xres;
    dpiy = (int)yres;

    m_Width = m_nTiffColumns = width;
    m_Height = m_nTiffRows = height;

    // Fillorder determines whether we convert on the fly or not, although
  // multistrip images also need to be converted
    TIFFGetField (image, TIFFTAG_FILLORDER, &fillorder);

    if ( (fillorder == FILLORDER_LSB2MSB) || (TIFFNumberOfStrips (image) > 1))
    {
    /*************************************************************************
      Convert the image
    *************************************************************************/

      //  OutputDebugString("Conversion of the image in-memory will occur.\n");

      // Because of the way this is implemented to integrate with the tiff lib
      // we need to ensure that we are the only thread that is performing this
      // operation at the moment. This is not a well coded piece of the library,
      // but I am at a loss as to how to do it better... We don't check if we
      // have already used global tiff buffer, because we are still using it's
      // old contents...
      m_vImgStream.resize(0);
      m_sTiffOffset = 0;

      // Open the dummy document (which actually only exists in memory)
      conv =
        TIFFClientOpen ("dummy", "w", (thandle_t) - 1,
            libtiffReadProc,
            libtiffWriteProc,
            libtiffSeekProc,
            libtiffCloseProc,
            libtiffSizeProc,
            NULL,
            NULL);

      // Copy the image information ready for conversion
      stripSize = TIFFStripSize (image);
      stripMax = TIFFNumberOfStrips (image);
      imageOffset = 0;

      stripBuffer.resize(TIFFNumberOfStrips (image) * stripSize);

      for (stripCount = 0; stripCount < stripMax; stripCount++)
      {
         imageOffset += static_cast<unsigned long>(TIFFReadEncodedStrip (image, stripCount,
                                              &stripBuffer[imageOffset],stripSize));
      }

      // We also need to copy some of the attributes of the tiff image
      // Bits per sample has to be 1 because this is going to be a G4/G3 image
      // (and all other image formats were stripped out above).
      twidth = width;

      theight = height;
      TIFFSetField (conv, TIFFTAG_IMAGEWIDTH, twidth);
      TIFFSetField (conv, TIFFTAG_IMAGELENGTH, theight);
      TIFFSetField (conv, TIFFTAG_BITSPERSAMPLE, 1);
      TIFFSetField (conv, TIFFTAG_COMPRESSION, compression);
      TIFFSetField (conv, TIFFTAG_PLANARCONFIG, PLANARCONFIG_CONTIG);
      TIFFSetField (conv, TIFFTAG_ROWSPERSTRIP, height + 1);
//      TIFFSetField (conv, TIFFTAG_PHOTOMETRIC, PHOTOMETRIC_MINISWHITE);
      TIFFSetField (conv, TIFFTAG_PHOTOMETRIC, PHOTOMETRIC_MINISBLACK);
      TIFFSetField (conv, TIFFTAG_FILLORDER, FILLORDER_MSB2LSB);
      TIFFSetField (conv, TIFFTAG_SAMPLESPERPIXEL, 1);
      TIFFSetField (conv, TIFFTAG_ORIENTATION, ORIENTATION_TOPLEFT);
      TIFFSetField (conv, TIFFTAG_RESOLUTIONUNIT, RESUNIT_INCH);
      TIFFSetField (conv, TIFFTAG_XRESOLUTION, (double)dpix);
      TIFFSetField (conv, TIFFTAG_YRESOLUTION, (double)dpiy);
      TIFFSetField (conv, TIFFTAG_SOFTWARE, "DynaRithmic TWAIN Library (DTWAIN) 5.0");

      if (compression == COMPRESSION_CCITTFAX4)
            TIFFSetField (conv, TIFFTAG_GROUP4OPTIONS, 0);

      // Actually do the conversion
      TIFFWriteEncodedStrip (conv, 0, stripBuffer.data(), imageOffset);

      // Finish up
      m_imgLengthInBytes = m_sTiffOffset;
    }

  else
    {
        //OutputDebugString("Image is not being converted internally\n");
    /**************************************************************************
       Insert the image
    **************************************************************************/

#if defined DEBUG
      printf ("Image is not being converted internally.\n");
#endif

      // We also need to add a binary stream to the object and put the image
      // data into this stream
      stripSize = TIFFStripSize (image);
      imageOffset = 0;

      int nStrips = TIFFNumberOfStrips(image);
      m_vImgStream.resize( nStrips * stripSize );
    }

    // Flate encode this stuff
    std::vector<char> tempRealData = m_vImgStream;
    EncodeVectorStream(tempRealData,
                       m_imgLengthInBytes,
                       m_vImgStream,
                       PdfDocument::FLATE_COMPRESS);

    m_imgLengthInBytes = m_vImgStream.size();
    unsigned long crcVal = crc32_aux(0, (unsigned char *)m_vImgStream.data(), (unsigned int)m_imgLengthInBytes);

    m_nCurCRCVal = crcVal;
    TIFFClose (image);
    TIFFClose (conv);
    return true;
}

void ImageObject::ComposeObject()
{

    char szBuf[255];
    SetContents("<< /Type /XObject\n"
                "   /Subtype /Image\n"
                "   /Name ");
    sprintf(szBuf, "/%s\n", m_sPDFImgName.c_str());
    AppendContents( szBuf );
    PdfDocument *pParent = GetParent();
	if (!pParent)
		return;

    // do a85 encoding here for a test
    if ( pParent->IsASCIICompressed() )
    {
        std::string sOut;
        std::string sIn;
        sIn.append(m_vImgStream.data(), m_vImgStream.size());
        ASCII85Encode(sIn, sOut);
        m_vImgStream.resize(sOut.size());
        std::copy(sOut.begin(), sOut.end(), m_vImgStream.begin());
        m_imgLengthInBytes = sOut.size();
        if ( m_nImgType == 0 )
            AppendContents("   /Filter [/ASCII85Decode /DCTDecode]\n");
        else
            AppendContents("   /Filter [/ASCII85Decode /FlateDecode]\n");
    }
    else
    {
        // Get the filter type to use
        if ( m_nImgType == 0 )
            AppendContents("   /Filter /DCTDecode\n");
        else
            AppendContents("   /Filter /FlateDecode\n");
    }
    sprintf(szBuf, "   /BitsPerComponent %d\n", m_bpp);
    AppendContents(szBuf);
    AppendContents("   /ColorSpace ");
    sprintf(szBuf, "/%s\n", m_sColorSpace.c_str());
    AppendContents(szBuf);
    sprintf(szBuf, "   /Width %d\n   /Height %d\n", m_Width, m_Height);
    AppendContents(szBuf);
    sprintf(szBuf, "   /Length %d ", static_cast<int>(m_imgLengthInBytes));
    AppendContents(szBuf);

    if (m_nImgType == 1 ) // Tiff
    {
        int polarity =pParent->GetPolarity();
        if (polarity == DTWAIN_PDFPOLARITY_POSITIVE )
            sprintf(szBuf,"/ImageMask true /Decode [0 1] ");
        else
            sprintf(szBuf,"/ImageMask true /Decode [1 0] ");
        AppendContents(szBuf);
    }
    AppendContents(">>\nstream\n");
    m_sExtraInfo = "";

    if ( pParent && pParent->IsEncrypted())
    {
        // make the extra info the current contents
        m_sExtraInfo = GetContents();
        // Encrypt the image stream
        m_sExtraInfoEnd = "\nendstream";
    }
    else
    {
        WriteRaw(m_vImgStream.data(), m_imgLengthInBytes);
        AppendContents("\nendstream");
    }
}

std::string ImageObject::GetStreamContents()
{
    string s;
    PdfDocument *pParent = GetParent();
    if ( pParent && pParent->IsEncrypted() )
    {
        s.assign(m_vImgStream.data(), m_imgLengthInBytes);
        return s;
    }
    return GetContents();
}

std::string ImageObject::GetExtraInfo()
{
    return m_sExtraInfo;
}

std::string ImageObject::GetExtraInfoEnd()
{
    return m_sExtraInfoEnd;
}

void ProcSetObject::ComposeObject()
{
    SetContents("[/ImageB /ImageC /ImgI /PDF /Text]");
}

void FontObject::ComposeObject()
{
    std::string s = "<< /Type /Font /Subtype /Type1 /BaseFont /";
    s += fontname + " /Encoding /WinAnsiEncoding >>";
    SetContents(s);
}

void PageObject::ComposeObject()
{
    char szBuf [100];
    string realmediabox = m_smediabox;
    string sConstantText;
    SetContents("<< /Type /Page\n"
                "   /Parent 2 0 R\n"
                "   /MediaBox ");
    AppendContents(realmediabox);
    AppendContents("\n");
    AppendContents("   /Contents ");
    sprintf(szBuf, "%d 0 R\n", GetObjectNum() + 1);
    AppendContents(szBuf);
    m_nMaxObjectNum = GetObjectNum() + 1;

    AppendContents("   /Resources  <</XObject <<");
    sConstantText = "   /Resources  <</XObject <<";

    sprintf (szBuf,"/Img%d ", static_cast<int>(m_nImageNum));
    AppendContents(szBuf);
    sConstantText += szBuf;

    if ( !m_bDuplicateImage )
    {
        sprintf(szBuf, "%d 0 R >>", GetObjectNum() + 2);
        m_nDuplicateObjNum = GetObjectNum() + 2;
    }
    else
        sprintf(szBuf, "%d 0 R >>", static_cast<int>(m_nDuplicateObjNum));
    m_nMaxObjectNum = max(m_nDuplicateObjNum, m_nMaxObjectNum);

    AppendContents(szBuf);
    sConstantText += szBuf;

    // Test placing font resource into map
    PdfDocument *pDoc = GetParent();
    if ( !pDoc->IsProcSetObjDone() )
    {
        pDoc->SetProcSetObjNum( GetObjectNum() + 3 );
        pDoc->SetProcSetObjDone( true );
    }

    sprintf(szBuf," /ProcSet %d 0 R \n", static_cast<int>(pDoc->GetProcSetObjNum()));
    AppendContents(szBuf);
    sConstantText += szBuf;

    m_nMaxObjectNum = max(m_nMaxObjectNum, pDoc->GetProcSetObjNum());

    if ( !pDoc->IsFontObjDone() )
    {
        pDoc->SetFontObjDone(true);
    }

    int nextObjNum = -1;

    // Now compose the contents object, since we now know the font ref numbers
    theContents->CreateFontDictAndText(m_nMaxObjectNum + 1, nextObjNum);
    theContents->SetImageName( szBuf );
    theContents->ComposeObject();

    m_nMaxObjectNum = nextObjNum - 1;
    std::string fontDict = theContents->GetFontDictionaryString();
    AppendContents(fontDict);
    AppendContents(">>\n");
    sConstantText += fontDict; //szBuf;
    sConstantText += ">>\n";
    sprintf(szBuf,">>");
    AppendContents(szBuf);
    sConstantText += szBuf;

    // Get the CRC for the page contents
    vector<char> v(sConstantText.begin(), sConstantText.end());
    m_CRCValue = crc32_aux(0, (unsigned char*)v.data(), (unsigned int)v.size());
}


bool PdfDocument::IsDuplicateImage(unsigned long CRCVal, unsigned long& ObjNum)
{
    CRCMapToObj::iterator it = m_allCRC.find( CRCVal );
    if ( it != m_allCRC.end())
    {
        ObjNum = (*it).second;
        return true;
    }
    return false;
}

void PdfDocument::AddDuplicateImage(unsigned long CRCVal, unsigned long ObjNum)
{
    m_allCRC[CRCVal] = ObjNum;
}


bool PdfDocument::IsDuplicatePage(unsigned long CRCVal, unsigned long& ObjNum)
{
    CRCMapToObj::iterator it = m_allPageCRC.find( CRCVal );
    if ( it != m_allPageCRC.end())
    {
        ObjNum = (*it).second;
        return true;
    }
    return false;
}

void PdfDocument::AddDuplicatePage(unsigned long CRCVal, unsigned long ObjNum)
{
    m_allPageCRC[CRCVal] = ObjNum;
}

void PdfDocument::SetEncryption(const CTL_StringType& ownerPassword,
                                const CTL_StringType& userPassword,
                                unsigned int permissions,
                                bool bIsStrongEncryption,
                                bool isAESEncryoted)
{
    m_EncryptionPassword[OWNER_PASSWORD] = StringConversion::Convert_Native_To_Ansi(ownerPassword);
    m_EncryptionPassword[USER_PASSWORD] = StringConversion::Convert_Native_To_Ansi(userPassword);
    m_nPermissions = permissions;
    m_bIsStrongEncryption = bIsStrongEncryption;
    m_bIsAESEncrypted = isAESEncryoted;

    // set the encryption to AES
    if (m_bIsAESEncrypted)
    {
/*      m_Encryption.reset(new PDFEncryptionAES);
        m_bIsStrongEncryption = true; // always uses strong encryption for AES

        // must set to PDF version 1.6
        SetPDFVersion(1,6);*/
    }

    bool bOk = true;

    // Keep going until we find keys that have valid chars
    do
    {
        std::string s = GetSystemTimeInMilliseconds().substr(0,13) + "+1359064+" + m_sCurSysTime.substr(0,13);
        std::string dID = CMD5Checksum().GetMD5((const unsigned char *)s.c_str(), (UINT)s.size());
        m_DocumentID[0] = dID;
        m_DocumentID[1] = dID;

        m_Encryption->SetupAllKeys(m_DocumentID[0], m_EncryptionPassword[USER_PASSWORD], m_EncryptionPassword[OWNER_PASSWORD], permissions, bIsStrongEncryption);
        bOk = true;
    } while (!bOk);

    m_bIsEncrypted = true;
}

PdfDocument::~PdfDocument() { }
