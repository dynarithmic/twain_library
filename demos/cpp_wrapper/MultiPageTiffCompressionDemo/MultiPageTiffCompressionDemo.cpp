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

// Derive from twain_callback to trap any notifications sent by
// the TWAIN retrieval process
class my_callback : public twain_callback
{
    int page = 1;
public:
    // Ready to transfer a file.  Change the TIFF compression type
    int transferready(twain_source& ts) override
    {
        // Odd numbered pages have no compression, even numbered pages are compressed 
        // using Group 4 FAX
        if (page % 2)
            ts.set_tiff_compress_type(tiffcompress_value::nocompress);
        else
            ts.set_tiff_compress_type(tiffcompress_value::group4);
        ++page;
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
            // we listen for the "transfer ready" notification during the acquisition process.
            // Once notified, we set the TIFF compression.
            session.register_callback(twsource, my_callback());

            // set the characteristics to acquire to a file.
            // By default, this will acquire to a Windows multipage TIFF file
            auto& ac = twsource.get_acquire_characteristics();
            ac.get_file_transfer_options().
               set_name("tiffdifferentcompressions.tif").set_type(filetype_value::tiffnocompressmulti);

            // enable the feeder (may need to do this for "devices" that do not 
            // use hardware to acquire images.
            ac.get_paperhandling_options().enable_feeder(true);

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