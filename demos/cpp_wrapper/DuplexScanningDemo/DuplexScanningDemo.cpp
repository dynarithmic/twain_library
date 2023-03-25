#include <iostream>
#include <string>
#include <dynarithmic/twain/twain_session.hpp> // for dynarithmic::twain::twain_session
#include <dynarithmic/twain/twain_source.hpp>  // for dynarithmic::twain::twain_source
#include <dynarithmic/twain/acquire_characteristics.hpp> // for acquire_characteristics
#include <dynarithmic/twain/capability_interface.hpp> // for capability_interface
#include "..\Runner\runnerbase.h"

struct Runner : RunnerBase
{
    int Run();
};

int Runner::Run()
{
    using namespace dynarithmic::twain;

    // Create a TWAIN session
    twain_session ts(startup_mode::autostart);

    // select a source (TWAIN session will automatically start)
    auto selection = ts.select_source();

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

        // see if duplex is supported
        auto& ci = twsource.get_capability_interface();
        if (!ci.is_duplex_supported())
        {
            std::cout << "Duplex is not supported for this device\n";
            return 0;
        }

        // set the duplex
        auto& ac = twsource.get_acquire_characteristics();
        ac.get_paperhandling_options().enable_duplex(true);

        // set the characteristics to acquire to a file.
        // Set to a TIFF-LZW file
        ac.get_file_transfer_options().
            set_name("duplex.tif").  // File name
            set_type(filetype_value::tifflzwmulti);  // set the file type to multipage TIFF-LZW

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
    return 1;
} // The twain_session ts will automatically close on exit of this function

int main()
{
    Runner().Run();
}