// ManualDuplexScanningDemo.cpp : This file contains the 'main' function. Program execution begins and ends there.
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

using namespace dynarithmic::twain;

// Derive from twain_callback to trap any notifications sent by
// the TWAIN retrieval process
class manual_duplex_callback : public twain_callback
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

    int manualduplexside1start(twain_source&) override 
    { 
        std::cout << "Front or Top side of papers are now about to be scanned\n";
        return 1; 
    }

    int manualduplexside2start(twain_source&) override
    { 
        std::cout << "Flip over pages in feeder to scan the back portion\n";
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
            // we listen for any errors during acquisition
            session.register_callback(twsource, manual_duplex_callback());

            auto& ac = twsource.get_acquire_characteristics();

            // Set the manual duplex mode
            auto& paperhandling = ac.get_paperhandling_options();

            // The way paper is fed may be different, so adjust the parameter to fit
            // your device
            paperhandling.set_duplexmode(manualduplexmode_value::manual_faceuptopfeed);

            // set the characteristics to acquire to a file.
            // Set to a multipage TIFF-LZW file
            ac.
                get_file_transfer_options().
                set_name("c:\\dtwain_ctest\\tifmulti_from_wrapper.tif").  // File name
                set_type(filetype_value::tifflzwmulti);

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