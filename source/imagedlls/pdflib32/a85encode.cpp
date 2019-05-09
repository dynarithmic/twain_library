#include "a85encode.h"
#include <algorithm>
#include <string>

class A85Encoder
{
    public:
        A85Encoder() : count(0), width(72), pos(0), tuple(0) { }
        std::string EncodeA85(const std::string& strIn);

    private:
        int count;
        unsigned long width, pos, tuple;
        void processA85char(unsigned c);
        void cleanup85();
        void encode(unsigned long, int count);
        std::string strOut;
};

std::string A85Encoder::EncodeA85(const std::string& strIn)
{
    strOut.clear();
    strOut.reserve(strIn.length());
    for (char ch : strIn) 
        processA85char(ch);
    cleanup85();
    return strOut;
}

void A85Encoder::processA85char(unsigned c)
{
    c = c & 0x00FF;
    switch (count++)
    {
        case 0: tuple |= (c << 24); break;
        case 1: tuple |= (c << 16); break;
        case 2: tuple |= (c <<  8); break;
        case 3:
            tuple |= c;
            if (tuple == 0)
            {
                strOut += 'z';
                if (pos++ >= width)
                {
                    pos = 0;
                    strOut += '\n';
                }
            }
            else
                encode(tuple, count);
            tuple = 0;
            count = 0;
        break;
    }
}

void A85Encoder::encode(unsigned long tupleParam, int countParam)
{
    int i;
    char buf[5], *s = buf;
    i = 5;
    do {
        *s++ = (char)(tupleParam % 85);
        tupleParam /= 85;
    } while (--i > 0);
    i = countParam;
    do
    {
        char ch = *--s;
        ch += '!';
        strOut += ch;
        if (pos++ >= width)
        {
            pos = 0;
            strOut += '\n';
        }
    } while (i-- > 0);
}

void A85Encoder::cleanup85(void)
{
    if (count > 0)
        encode(tuple, count);
    if (pos + 2 > width)
        strOut += '\n';
    strOut += "~>";
}

int ASCII85Encode( const std::string&inData, std::string& outData)
{
    A85Encoder encoder;
    outData = encoder.EncodeA85(inData);
    return 1;
}
