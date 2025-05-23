/*
This file is part of the Dynarithmic TWAIN Library (DTWAIN).
Copyright (c) 2002-2025 Dynarithmic Software.

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
// This class is used to listen for TWAIN events when an acquisition is occurring.  Use this to do special
// processing on acquisition startup, UI opening and closing, etc.  See the DTWAIN help manual for a list of
// the various events.

#include <string>
#include <algorithm>
#include <cctype>
#include <dynarithmic/twain/utilities/string_utilities.hpp>

#ifdef _MSC_VER
	#pragma warning( push )  // Stores the current warning state for every warning.
	#pragma warning( disable:4996)
#endif

namespace dynarithmic
{
	namespace twain
	{
		std::string& ltrim(std::string& str)
		{
			auto it2 = std::find_if(str.begin(), str.end(), [](unsigned char ch)
				{ return !isspace(ch); });
			str.erase(str.begin(), it2);
			return str;
		}

		std::string& rtrim(std::string& str)
		{
			auto it1 = std::find_if(str.rbegin(), str.rend(), [](unsigned char ch)
				{ return !isspace(ch); });
			str.erase(it1.base(), str.end());
			return str;
		}

		std::string ltrim_copy(std::string str)
		{
			return ltrim(str);
		}

		std::string rtrim_copy(std::string str)
		{
			return rtrim(str);
		}

		std::string trim_copy(std::string str)
		{
			return ltrim(rtrim(str));
		}

		std::string& trim(std::string& str)
		{
			return ltrim(rtrim(str));
		}
	}
}
#ifdef _MSC_VER
	#pragma warning(pop)
#endif
