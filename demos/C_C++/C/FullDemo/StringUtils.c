#include <ctype.h>
#include <string.h>
#include "StringUtils.h"

int StringToBoolInt(const char* s)
{
	char c;

	/* Skip leading spaces */
	while (*s && isspace((unsigned char)*s))
		++s;

	c = (char)toupper((unsigned char)*s);

	/* TRUE or 1 */
	if (c == 'T' || c == '1')
		return 1;

	/* FALSE or 0 */
	return 0;
}

static void ReplaceCRLFWithSpaceCopy(const char* input,char* output,size_t outSize)
{
	size_t i = 0;

	while (*input && i + 1 < outSize)
	{
		if (input[0] == '\r' && input[1] == '\n')
		{
			output[i++] = ' ';
			input += 2;
		}
		else
		{
			output[i++] = *input++;
		}
	}

	output[i] = '\0';
}

int ParseTextBySpaces(const char* text,TokenCallback tokenCb, int noParse, void* userData)
{
	const char* p = NULL;

	if (!text || !tokenCb)
		return 1;

	char textCopy[2000];
	ReplaceCRLFWithSpaceCopy(text, textCopy, 1999);

	p = textCopy;

	if (noParse)
	{
		tokenCb(p, userData);
		return 1;
	}

	while (*p)
	{
		/* Skip leading whitespace */
		while (*p && isspace((unsigned char)*p))
			++p;

		if (!*p)
			break;

		const char* start = p;

		/* Find end of token */
		while (*p && !isspace((unsigned char)*p))
			++p;

		size_t len = (size_t)(p - start);

		char token[256];
		if (len >= sizeof(token))
			len = sizeof(token) - 1;

		memcpy(token, start, len);
		token[len] = '\0';

		{
			int retVal = tokenCb(token, userData);
			if (retVal == 0)
				break;
		}
	}
	return 1;
}

