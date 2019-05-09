#include <ahexencode.h>

int ASCIIHexEncode( const std::string&inData, std::string& outData)
{
    const char *pHexStr = "0123456789ABCDEF";
    unsigned int hival, loval;
    outData.reserve(inData.size() * 2);
    for ( size_t i = 0; i < inData.size(); ++i)
    {
        unsigned char ch = inData[i];
        ch = ch >> 4;
        hival = ch;
        loval = inData[i] & 0x0F;
        outData += pHexStr[hival];
        outData += pHexStr[loval];
    }
    outData += '>';
    return 1;
}
