#ifndef TOJSON_H
#define TOJSON_H

#include <string>
#include <vector>

namespace dynarithmic
{
    namespace twain
    {
        class twain_session;
        class twain_source;
        class json_generator
        {

        public:
            static std::string generate_details(twain_session& ts, const std::vector<std::string>& allSources, bool bWeOpenSource = false);
        };
    }
}
#endif