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
#include "a85decoder.h"

#include <cstdio>
void A85Decoder::wput(std::string & output, unsigned long tuple, int bytes)
{
    char buf[100];
    static const int shifter[] = {24, 16, 8, 0};
    if ( bytes >=1 && bytes <= 4 )
    {
        for (int i = 0; i < bytes; ++i )
        {
            sprintf(buf, "%c", (unsigned char)(tuple >> shifter[i]));
            output+=buf;
        }
    }
}

std::string A85Decoder::decode85()
{
    unsigned long tuple = 0;
    int c, count = 0;
    int curinputPos = 0;
    std::string output;
    while (curinputPos < static_cast<int>(m_scratch.size()))
    {
        c = m_scratch[curinputPos++];
        switch (c)
        {
        default:
            if (c < '!' || c > 'u')
                return "";
            tuple += (c - '!') * pow85[count++];
            if (count == 5)
            {
                wput(output, tuple, 4);
                count = 0;
                tuple = 0;
            }
            break;

        case 'z':
            if (count != 0)
                return "";
            output.append(4,'\0');
            break;

        case '~':
            if ( m_scratch[curinputPos++] == '>')
            {
                if (count > 0)
                {
                    count--;
                    tuple += pow85[count];
                    wput(output, tuple, count);
                }
                c = m_scratch[curinputPos++];
                return output;
            }
        case '\n': case '\r': case '\t': case ' ':
        case '\0': case '\f': case '\b': case 0177:
            break;
        }
    }
    return output;
}

std::string A85Decoder::DecodeA85(const std::string& strIn,
                                  bool checkPrefix,
                                  bool checkSuffix
                                  )
{
    if (strIn.size() < 2)
        return "";
    m_scratch = strIn;
    if ( checkPrefix )
    {
        if (strIn[0] != '<' || strIn[1] != '~' )
            return "";
        m_scratch = strIn.substr(2);
    }
    if (checkSuffix)
    {
        if (strIn[strIn.size()-2] != '~' ||
            strIn[strIn.size()-1] != '>')
            return "";
    }
    return decode85();
}
