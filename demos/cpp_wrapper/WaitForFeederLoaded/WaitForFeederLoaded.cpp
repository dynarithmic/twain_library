#include <iostream>
#include <string>
#include <dynarithmic/twain/twain_session.hpp> // for dynarithmic::twain::twain_session
#include <dynarithmic/twain/twain_source.hpp>  // for dynarithmic::twain::twain_source
#include <dynarithmic/twain/acquire_characteristics.hpp>  // for acquire_characteristics
#include <dynarithmic/twain/capability_interface.hpp>  // for capability_interface
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
            return 0;
        }

        // open the source from the selection above    
        twain_source twsource(selection);

        // check if we were able to open the source
        if (twsource.is_open())
        {
            // output the source product name
            std::cout << twsource.get_source_info().get_product_name() << "\n";

            // We will only run this demo for devices that have a document feeder
            // that can detect if paper has been inserted.  We didn't have to return
            // but for the sake of this demo, we will only continue to show the 
            // affect of waiting for the feeder.
            auto& ci = twsource.get_capability_interface();
            if (!ci.is_paperdetectable_supported())
                return 0;

            // get the file saving characteristics
            auto& ac = twsource.get_acquire_characteristics();

            // Set the paper handling options
            auto& paperhandling = ac.get_paperhandling_options();

            // We will wait for 30 seconds for the feeder to be fed paper.  After 30 seconds
            // if no paper has been detected in the feeder, the flatbed will be used instead of the feeder.  
            //
            // If the device did not have a feeder, or if the automatic feeder could not detect if 
            // paper is loaded, then we would have just used the flatbed or the feeder, without waiting 30 seconds.
            //
            // To have an infinite wait time, use paperhandling::wait_infinite in the call to
            // set_feederwait().
            paperhandling.enable_feeder(true).set_feederwait(30).  // wait time
                          set_feedermode(feedermode_value::feeder_flatbed); // feeder, then flatbed

            auto& ftOpts = ac.get_file_transfer_options();
            // set the characteristics to acquire to a file.
            // Set to a TIFF-LZW file
            ftOpts.set_name("tif_from_wrapper.tif").  // File name
                   set_type(filetype_value::tifflzwmulti);  // set the file type to a multipage TIFF-LZW

            // We will only acquire 2 pages
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