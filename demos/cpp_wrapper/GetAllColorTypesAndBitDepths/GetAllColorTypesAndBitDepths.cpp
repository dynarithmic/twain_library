// GetAllColorTypesAndBitDepths.cpp : Defines the entry point for the console application.
//
#include <iostream>
#include <string>
#include <vector>
#include <dynarithmic/twain/twain_session.hpp> // for twain_session
#include <dynarithmic/twain/twain_source.hpp>  // for twain_source
#include <dynarithmic/twain/capability_interface.hpp>  // for capability_interface

using namespace dynarithmic::twain;
struct Runner
{
    int Run();
    ~Runner()
    {
        printf("\nPress Enter key to exit application...\n");
        char temp;
        std::cin.get(temp);
    }
};

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
            // output the source product name
            std::string prodName = twsource.get_source_info().get_product_name();
            std::cout << prodName << "\n";

            // Get the interface to the capabilities of the device
            auto& ci = twsource.get_capability_interface();

            // Get all of the supported pixel types (the pixel types are the various color options)
            auto vPixelTypes = ci.get_pixeltype();
            if (vPixelTypes.empty())
            {
                std::cout << "Could not obtain the color type information for device \"" << prodName << "\"\n";
                return 0;
            }

            std::cout << "Pixel type and bit depth info for device \"" << 
                        twsource.get_source_info().get_product_name() + "\":\n";

            // Loop through the pixel types, and get the supporting bit depth(s) for each
            // pixel type
            for (auto colorType : vPixelTypes)
            {
                std::cout << "\nColor Type " << 
                    twain_session::get_twain_name(twain_constant_category::TWPT, colorType) << 
                    " has the following supported bit depths:\n";
                // set the pixel type
                ci.set_pixeltype({colorType});
                if (ci.get_last_error().error_code == DTWAIN_NO_ERROR)
                {
                    // Get the bit depth values associated with the selected pixel type
                    auto vBitDepths = ci.get_bitdepth();
                    int current = 1;
                    for (auto bd : vBitDepths)
                    {
                        std::cout << "BitDepth " << current << ": " << bd << "\n";
                        ++current;
                    }
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
