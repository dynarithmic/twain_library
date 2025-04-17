// BarCodeDemo.cpp : This file contains the 'main' function. Program execution begins and ends there.
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
class barcode_callback : public twain_callback
{
public:

    // Obtaining bar codes is done on each successful transfer (state 7).
    int transferdone(twain_source& ts) override
    {
        // Initialize the extended image information 
        // (bar codes, patch codes, etc.)
        auto extInfo = ts.init_extendedimage_info();

        // Note that extInfo is a std::unique_ptr<extended_info>, thus is only
        // valid in this local {} scope.  To get a "permanent" copy, use the
        // static extendedimage_info::clone() function.
        if (extInfo)
        {
            // extended image information retrieved, now get the bar code data

            // general bar code information
            const auto& barcodeinfo = extInfo->get_barcode_info();
            auto numBarCodes = barcodeinfo.get_count();

            // individual bar code information
            const auto& barcodes = extInfo->get_barcodes();

            // Output information on  bar codes found
            std::cout << "Number of bar codes: " << numBarCodes << "\n\n";
            std::cout << "Bar code info:\n";
            int curInfo = 1;
            for (auto& info : barcodes)
            {
                std::cout << "Bar code " << curInfo << ":\n";
                std::cout << "    Text: " << info.get_text() << "\n";
                std::cout << "    Length: " << info.get_length() << "\n";
                std::cout << "    Position (x,y): (" << info.get_xCoordinate() << "," << info.get_yCoordinate() << ")\n";
                std::cout << "    Rotation: " << (barcodeinfo.is_rotation_supported() ?
                                                  std::to_string(info.get_rotation()) : " (not supported) ") << "\n";
                std::cout << "    Confidence: " << (barcodeinfo.is_confidence_supported() ?
                                                    std::to_string(info.get_confidence()) : " (not supported) ") << "\n";
                std::cout << "    Type: " << info.get_typename() << "\n\n";
                ++curInfo;
            }
        }
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
            // check if bar code support is available
            auto& capInterface = twsource.get_capability_interface();
            if (!capInterface.is_supportedbarcodetypes_supported())
            {
                std::cout << "barcode support is not available for device: " << twsource.get_source_info().get_product_name() << "\n";
                return 1;
            }
            // we set the bar code callback here
            session.register_callback(twsource, barcode_callback());

            // set the characteristics to acquire to a file.
            // By default, this will acquire to a Windows BMP file
            twsource.get_acquire_characteristics().
                get_file_transfer_options().
                set_name("barcode.bmp");  // Name of the file 

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