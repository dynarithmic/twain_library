#include <iostream>
#include <string>
#include <dynarithmic/twain/twain_session.hpp> // for dynarithmic::twain::twain_session
#include <dynarithmic/twain/twain_source.hpp>  // for dynarithmic::twain::twain_source
#include <dynarithmic/twain/acquire_characteristics.hpp>  // for acquire_characteristics
#include "..\Runner\runnerbase.h"

struct Runner : RunnerBase
{
    int Run();
};


using namespace dynarithmic::twain;

int Runner::Run()
{
    // Create a TWAIN session and automatically open the TWAIN data source manager
    // Note that we do not automatically start the session, since we want to set
    // up the logger before the TWAIN session starts.
    twain_session session;

    // create a logger and set the twain session to use the logger
    session.register_logger(twain_logger().enable().
                            set_destination(logger_destination::toconsole). // log to the console
                            set_verbosity(logger_verbosity::verbose2). // verbosity level
                            enable_custom(true). // enable the custom log destination to our logger's custom handler
                            set_custom_function([](const char* msg)
                                {
                                    std::string s = "I got a message: " + std::string(msg) + "\n";
                                    OutputDebugStringA(s.c_str());
                                }));
    // Start the session
    session.start();

    // select a source
    auto selection = session.select_source();

    // check if user canceled the selection
    if (selection.canceled())
    {
        std::cout << "User canceled selecting the source\n";
        return 0;
    }

    // open the source from the selection above    
    twain_source twsource(selection);

    // check if we were able to open the source
    if (twsource.is_open())
    {
        // output the source product name
        std::cout << twsource.get_source_info().get_product_name() << "\n";

        // set the characteristics to acquire to a file.
        // By default, this will acquire to a Windows BMP file
        twsource.get_acquire_characteristics().
            get_file_transfer_options().
            set_name("logger_demo.bmp");  // Name of the file

        // Start the acquisition
        auto retval = twsource.acquire();

        // If there is an internal error, get the error
        if (twsource.acquire_internal_error(retval.first))
            std::cout << twain_session::get_error_string(twain_session::get_last_error());
        else
            // Check if user scanned and/or canceled
            std::cout << (retval.first == twsource.acquire_canceled ? "Canceled" : "OK");
    }
    else
    {
        // Could not open the source for some reason.
        std::cout << twain_session::get_error_string(twain_session::get_last_error());
    }
    return 1;
} // The twain_session ts will automatically close on exit of this function

int main()
{
    Runner().Run();
}