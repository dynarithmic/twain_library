// SinglePageDeviceToMultiPageDemo.cpp : This file contains the 'main' function. Program execution begins and ends there.
//
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

            // get the file saving characteristics
            auto& ac = twsource.get_acquire_characteristics();

            // Set the multipage save options to save when the source UI is closed
            auto& ftOpts = ac.get_file_transfer_options();
            ftOpts.get_multipage_save_options().set_save_mode(multipage_save_mode::save_uiclose);

            // set the characteristics to acquire to a file.
            // Set to a multi-page TIFF-LZW file
            ftOpts.set_multi_page(true). // This must always be explicitly turned on!!
                   set_name("c:\\dtwain_ctest\\tif_from_wrapper.tif").  // File name
                   set_type(filetype_value::tifflzwmulti);  // set the file type to a multipage TIFF-LZW
            ac.get_general_options().set_max_page_count(2);

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
} // The twain_session session will automatically close on exit of this function

int main()
{
    Runner().Run();
}