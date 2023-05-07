// GetSetCapabilities.cpp : This file contains the 'main' function. Program execution begins and ends there.
//
#include <iostream>
#include <string>
#include <vector>
#include <dynarithmic/twain/twain_session.hpp> // for dynarithmic::twain::twain_session
#include <dynarithmic/twain/twain_source.hpp>  // for dynarithmic::twain::twain_source
#include <dynarithmic/twain/capability_interface.hpp>  // for capability_interface
#include "..\Runner\runnerbase.h"

struct Runner : RunnerBase
{
    int Run();
};

using namespace dynarithmic::twain;

int Runner::Run()
{
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
            // Get the interface to the capabilities of the device.
            // Note that the capability interface actually will "talk" 
            // to the device, getting and setting the capabilities.  This differs
            // from the acquire characteristics, which only logically sets values
            // without talking to the actual device.
            auto& ci = twsource.get_capability_interface();

            // Get the paper sizes using the provided supportedsizes() function
            auto allPaperSizes = ci.get_supportedsizes();

            // Get all the paper sizes, but use the ICAP_SUPPORTEDSIZES directly.
            // Note that we can use the general get_cap_values function if we want
            // to provide the capability to set as an explicit argument instead of
            // calling the specific capability setting function.
            auto allPaperSizes2 = ci.get_cap_values(ICAP_SUPPORTEDSIZES);

            // Both vectors that are returned should be exactly the same.  The get_supportedsizes() and
            // get_cap_values(ICAP_SUPPORTEDSIZES) do exactly the same thing.  The former
            // is more explicit as to what is being retrieved, the latter is more flexible, since
            // get_cap_values() allows setting of custom capabilities, but
            // requires you to know the TWAIN constant to use.
            if (allPaperSizes != allPaperSizes2)
                std::cout << "Something went wrong!\n";

            int i = 0;
            for (auto& val : allPaperSizes)
            {
                std::cout << "Page size " << i + 1 << ": " <<
                    session.get_twain_name(twain_constant_category::TWSS, val) << "\n";
                ++i;
            }

            // Set the current size to the first value found 
            // Note that to set capabilities, the first argument is always a container
            // of values, and not a single value. 
            // 
            ci.set_supportedsizes(allPaperSizes); // default set type is "capability_interface::set()"
            // 
            // Note that we also could have explicitly stated the values in a temporary container as 
            // the first argument:
            // 
            // ci.set_supportedsizes({ allPaperSizes.front() }); 
            // 
            // Note the brace initialization to create a temporary container:
            //
            // For the generic capability setting function, note that we could have stated:
            // 
            // ci.set_cap_values(allPaperSizes, ICAP_SUPPORTEDSIZES, capability_interface::set());
            // 
            // or
            // 
            // ci.set_cap_values(allPaperSizes, ICAP_SUPPORTEDSIZES);
            // 
            // where the default set type (capability_interface::set()) will always use the first value
            // in the container of values to use.  

            // See if the current value actually has been set 
            auto currentSize = ci.get_supportedsizes(ci.get_current());
            if (currentSize.front() == allPaperSizes.front())
                std::cout << "Set the capability ok\n";
            else
                std::cout << "Did not set the capability\n";

            // Now for strings.  Test if the CAP_SERIALNUMBER capability is supported
            if (ci.is_serialnumber_supported())
            {
                // Get the serial number
                auto serialNumber = ci.get_serialnumber();
                if ( !serialNumber.empty() )
                    std::cout << "The serial number \"" << serialNumber.front() << "\"\n";
            }

            // Now for the odd one, frames.  Test the ICAP_FRAMES capability
            if (ci.is_frames_supported())
            {
                // Get the frames.  Returned will be a container of twain_frame<double> type
                auto allFrames = ci.get_frames();
                if (!allFrames.empty())
                {
                    twain_frame<double>& frame = allFrames.front();
                    // Write out the frame
                    std::cout << "\nLeft: " << frame.left << "\nTop: " << frame.top << "\nRight: " << frame.right
                        << "\nBottom: " << frame.bottom << "\n";

                    // Set the frame 
                    frame = { 0, /*left*/ 
                              0, /*top*/
                              4.5, /*right*/ 
                              8 /*bottom*/};

                    std::cout << "The frame we will try to set:\n";
                    std::cout << "\nLeft: " << frame.left << "\nTop: " << frame.top << "\nRight: " << frame.right
                        << "\nBottom: " << frame.bottom << "\n\n";

                    ci.set_frames({ frame });

                    // See if the frame was set
                    auto newFrames = ci.get_frames(ci.get_current());

                    if (newFrames.empty())
                    {
                        std::cout << "Getting current frame was not successful.\n";
                        return 1;
                    }

                    // Check if the frames are the same
                    if (newFrames.front() == frame)
                        std::cout << "The frame was set successfully\n";
                    else
                        std::cout << "The frame was not set successfully\n";

                    // Write out the frame
                    auto& testFrame = newFrames.front();

                    std::cout << "\nLeft: " << testFrame.left << "\nTop: " << testFrame.top << "\nRight: " << testFrame.right
                        << "\nBottom: " << testFrame.bottom << "\n";

                }
            }
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
