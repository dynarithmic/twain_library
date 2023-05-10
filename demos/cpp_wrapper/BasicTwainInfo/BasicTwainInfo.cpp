#include <iostream>
#include <string>
#include <vector>
#include <dynarithmic/twain/twain_session.hpp> // for dynarithmic::twain::twain_session
#include <dynarithmic/twain/twain_source.hpp>  // for dynarithmic::twain::twain_source
#include "..\Runner\runnerbase.h"

struct Runner : RunnerBase
{
    int Run();
};

/* Change this to the output directory that fits your environment */
int Runner::Run()
{
    using namespace dynarithmic::twain;

    // Create a TWAIN session 
    twain_session session(startup_mode::autostart);

    // Now check if session was started successfully.  
    if (session)
    {
        // Get the TWAIN information 
        std::cout << "DTWAIN Short Version Info: " << session.get_short_version_name() << "\n";
        std::cout << "DTWAIN Long Version Info: " << session.get_long_version_name() << "\n";
        std::cout << "DTWAIN Library Path: " << session.get_dtwain_path() << "\n";
        std::cout << "TWAIN DSM Path in use: " << session.get_dsm_path() << "\n";
        std::cout << "DTWAIN Version & Copyright: " << session.get_version_copyright() << "\n";

        // Get information on the installed TWAIN sources
        std::cout << "\nAvailable TWAIN Sources:\n";
        auto vectSources = session.get_all_source_info();  
        for (auto& source : vectSources)
            std::cout << "    Product Name: " << source.get_product_name() << "\n";

        // Select a source and display the information using JSON format
        twain_source source = session.select_source();

        if (source.is_selected())
        {
            std::string sDetails = source.get_details();
            std::cout << sDetails;
        }

        return 0;
    }
    else
    {
        // Did not load the DSM.  Cannot start TWAIN system, let's get the error string
        std::cout << twain_session::get_error_string(twain_session::get_last_error());
    }
    return -1;
}

int main()
{
    Runner().Run();
}