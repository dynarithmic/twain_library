// AutoFileNamingDemo.cpp : Defines the entry point for the console application.
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
        auto selection = RunnerBase::SelectDialog(session);

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
            // set the characteristics to acquire to a file.
            // Set to a TIFF-LZW file.
            // For each page acquired, the filename that will be saved will have an incrementing
            // number at the end of the filename.  For example:
            // tiff00000.tif, tiff0001.tif, tiff0002, etc.
            //
            // For tiff00000.tif, we can have up to 100000 files starting with name tiff00000.tif and
            // going up to tiff99999.tif.  

            // Get the file transfer options
            auto& file_options = twsource.get_acquire_characteristics().get_file_transfer_options();

            // Set the initial name and the file type
            file_options.set_name("tiff00000.tif").  // Initial file name
                         set_type(filetype_value::tifflzw);  // set the file type to TIFF-LZW

            // Get the rules of how we want to increment the filename, and enable the file naming option
            file_options.get_filename_increment_options().enable(true).set_increment(1); // we increment the number by 1 for each page

            // Start the acquisition process.
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
