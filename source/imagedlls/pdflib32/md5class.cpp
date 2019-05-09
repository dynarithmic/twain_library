#include <md5class.h>
#include <string.h>

std::vector<unsigned char> MD5::ComputeHash(const char *input, int len/* = -1*/)
{
  std::vector<unsigned char> digest(16);
  MD5Init(&context);
  int nLen = len;
  if ( len == -1 )
      nLen = static_cast<int>(strlen(input));
  MD5Update(&context, (unsigned char *)input, nLen);
  MD5Final(&digest[0], &context);
  return digest;
}

std::vector<unsigned char> MD5::ComputeHash()
{
  std::vector<unsigned char> digest(16);
  MD5Final(&digest[0], &context);
  return digest;
}

void MD5::Update(const char *input, int offset/* = 0*/, int len/* = -1*/)
{
  int nLen = len;
  if ( len == -1 )
      nLen = static_cast<int>(strlen(input));
  MD5Update(&context, (unsigned char *)(input + offset), nLen);
  m_inputdata.assign(input+offset, nLen);
}


std::vector<unsigned char> MD5::Final()
{
    std::vector<unsigned char> digest(16);
    MD5Final(&digest[0], &context);
    return digest;
}
