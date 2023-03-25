#include <iostream>
#include <string>
#include <dynarithmic/twain/twain_session.hpp> // for dynarithmic::twain::twain_session
#include <dynarithmic/twain/twain_source.hpp>  // for dynarithmic::twain::twain_source
#include <dynarithmic/twain/twain_callback.hpp>  // for twain_callback
#include <dynarithmic/twain/options/blankpage_options.hpp> // for the blank page options values
#include <dynarithmic/twain/acquire_characteristics.hpp>  // for the acquire_characteristics
#include "..\Runner\runnerbase.h"

struct Runner : RunnerBase
{
    int Run();
};

using namespace dynarithmic::twain;

// Derive from twain_callback to trap any notifications sent by
// the TWAIN retrieval process
class blankpage_callback : public twain_callback
{
public:

    int blankpagedetected_original(twain_source&) override
    {
        std::cout << "Detected blank page from device\n";

        // If you want to keep this page, return blankpage_options::keep_page
        return blankpage_options::discard_page;  // discard the page
    }

    int blankpagedetected_resampled(twain_source&) override
    {
        std::cout << "Detected blank page after resampling image\n";

        // If you want to keep this page, return blankpage_options::keep_page
        return blankpage_options::discard_page; // discard the page
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

            // we listen for notifications concerning detected blank pages
            session.register_callback(twsource, blankpage_callback());

            // set the characteristics to acquire to a file.
            // By default, this will acquire to a Windows BMP file
            twsource.get_acquire_characteristics().
                get_file_transfer_options().
                set_name("bmp_with_blank_pages_removed.bmp");  // Name of the file

            // Get the blank page options
            auto& blankpageoptions = twsource.get_acquire_characteristics().get_blank_page_options();

            // Enable blank page handling, and make sure we are notified if a page is blank by
            // setting the discard_on_notification flag
            blankpageoptions.enable(true).
                    set_discard_option(blankpage_discard_option::discard_on_notification).
                    set_threshold(95); // This will discard pages that are at least 95% blank
            
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