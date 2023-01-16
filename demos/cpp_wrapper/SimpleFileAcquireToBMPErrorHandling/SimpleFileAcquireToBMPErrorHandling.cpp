// SimpleFileAcquireToBMPErrorHandling.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <string>
#include <dynarithmic/twain/twain_session.hpp> // for dynarithmic::twain::twain_session
#include <dynarithmic/twain/twain_source.hpp>  // for dynarithmic::twain::twain_source
#include <dynarithmic/twain/acquire_characteristics.hpp>  // for acquire_characteristics

using namespace dynarithmic::twain;
struct Runner
{
    int Run();
    ~Runner()
    {
        printf("\nPress Enter key to exit application...\n");
        char temp;
        std::cin.get(temp);
    }
};

// Derive from twain_callback to trap any notifications sent by
// the TWAIN retrieval process
class error_callback : public twain_callback
{
    public:
        // Error saving the file
        int filesaveerror(twain_source& ts) override
        {
            auto errMsg = twain_session::get_error_string(twain_session::get_last_error());
            std::cout << "Could not save file.  Reason: " << errMsg << "\n";
            return 1;
        }

        // File saved ok
        int filesaveok(twain_source& ts) override
        {
            std::cout << "File saved ok\n";
            return 1;
        }
};

int Runner::Run()
{

    // Create a TWAIN session and automatically open the TWAIN data source manager
    twain_session session(startup_mode::autostart);

    // In this example, we will test whether the session was started successfully
    if (session)
    {
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

            // we listen for any errors during acquisition
            session.register_callback(twsource, error_callback());

            // set the characteristics to acquire to a file.
            // By default, this will acquire to a Windows BMP file
            twsource.get_acquire_characteristics().
                        get_file_transfer_options().
                            set_name("bmp_from_wrapper2.bmp");  // Name of the file

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
    }
    else
    {
        // Did not load the DSM.  Cannot start TWAIN system, let's get the error string
        std::cout << twain_session::get_error_string(twain_session::get_last_error());
    }
    return 1;
} 

int main()
{
    Runner().Run();
}