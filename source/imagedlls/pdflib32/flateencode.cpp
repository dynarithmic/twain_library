#include <algorithm>
#include <string>
#include "zlib.h"
#include "flateencode.h"

int FlateEncode(const std::string&inData, std::string& outData)
{
    unsigned long compressedLen = (long)((double)inData.size() * 1.2 + 12);
    outData.resize(compressedLen);
    int result = compress2((unsigned char *)&outData[0], &compressedLen,(const unsigned char *)&inData[0], (uLong)inData.size(), 9);
    outData.resize(compressedLen);
    return result;
}
