#ifndef STRINGUTILS_H
#define STRINGUTILS_H

int StringToBoolInt(const char* input);
typedef int (*LineCallback)(const char* line, void* userData);
typedef int (*TokenCallback)(const char* token, void* userData);

int ParseTextBySpaces(const char* text, TokenCallback tokenCb, int noParse, void* userData);

#endif