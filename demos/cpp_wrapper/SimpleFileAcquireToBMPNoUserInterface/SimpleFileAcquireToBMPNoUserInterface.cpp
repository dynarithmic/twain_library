#include <iostream>
#include <string>
#include <dynarithmic/twain/twain_session.hpp> // for dynarithmic::twain::twain_session
#include <dynarithmic/twain/twain_source.hpp>  // for dynarithmic::twain::twain_source
#include <dynarithmic/twain/acquire_characteristics.hpp>  // for acquire_characteristics
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

int Runner::Run()
{
    using namespace dynarithmic::twain;

    // Create a TWAIN session and automatically open the TWAIN data source manager
    twain_session session(startup_mode::autostart);

    // Now check if session was started successfully.  
    if (session)
    {
        // select a source
        auto selection = session.select_source();

        // check if user canceled the selection
        if (selection.canceled())
        {
            std::cout << "User canceled selecting the source\n";
            return 1;
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
            auto& ac = twsource.get_acquire_characteristics();
            ac.get_file_transfer_options().
               set_name("bmp_from_wrapper.bmp");  // Name of the file

            // Turn off the user-interface to the device
            ac.get_userinterface_options().show(false);

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
    return 0;
} // The twain_session ts will automatically close on exit of this function

int main()
{
    Runner().Run();
}