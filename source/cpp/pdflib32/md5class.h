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
#ifndef MD5CLASS_H
#define MD5CLASS_H
#define PROTOTYPES 1
#define HAVE_PROTOTYPES

#include <md5-global.h>
#include <md5c.h>
#include <vector>
#include <string>
class MD5
{
    public:
        MD5() : context{}  { }

        std::vector<unsigned char> ComputeHash(const char *input, int len = -1);
        std::vector<unsigned char> ComputeHash();
        void Reset() {   MD5Init(&context); }
        void Update(const char *input, int offset = 0, int len = -1);
        std::vector<unsigned char> Final();

    private:
        MD5_CTX context;
        std::string m_inputdata;
};
#endif