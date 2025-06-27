// ShowCustomDSData.cpp : This file contains the 'main' function. Program execution begins and ends there.
//
#include <iostream>
#include <string>
#include <deque>
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
        // output the source product name
        std::cout << twsource.get_source_info().get_product_name() << "\n\n";
        auto cData = twsource.get_custom_data();
        if (cData.empty())
            std::cout << "Device does not support custom DS data.\n";
        else
        {
            std::cout << "There are " << cData.size() << " bytes of custom DS data\n";
            std::cout << "The custom data for device is:\n";
            std::cout.write((char*)cData.data(), cData.size());

            std::cout << "\n\nNow will show the UI that will allow changes (but not acquire images):\n\n";
            twsource.showui_only();
        }
    }
    else
    {
        // Could not open the source for some reason.
        std::cout << twain_session::get_error_string(twain_session::get_last_error());
    }
    return 1;
} // The twain_session ts will automatically close on exit of this function

int main()
{
    Runner().Run();
}