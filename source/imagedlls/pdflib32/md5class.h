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
        MD5() { }

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