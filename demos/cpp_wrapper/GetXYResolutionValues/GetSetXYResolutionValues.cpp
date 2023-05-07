// GetAllColorTypesAndBitDepths.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <string>
#include <vector>
#include <dynarithmic/twain/twain_session.hpp> // for dynarithmic::twain::twain_session
#include <dynarithmic/twain/twain_source.hpp>  // for dynarithmic::twain::twain_source
#include <dynarithmic/twain/capability_interface.hpp>  // for capability_interface
#include <dynarithmic/twain/acquire_characteristics.hpp>  // for acquire_characteristics
#include "..\Runner\runnerbase.h"

struct Runner : RunnerBase
{
    int Run();
};

using namespace dynarithmic::twain;

const char * unit[] = { "Dots per Inch", "Dots per centimeter", "Picas", "Points", "TWIPS", "Pixels" };

template <typename dataType>
void PrintResolutionValues(const std::vector<dataType>& aResValues, LONG unitOfMeasure, char whichRes)
{
    int i = 0;
    for (auto& val : aResValues)
    { 
       std::cout << "  " << whichRes << " Resolution value " << i + 1 << ": " <<  val << " " << unit[unitOfMeasure] << "\n";
       ++i;
    }
    std::cout << "\n\n";
}

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
            // Get the interface to the capabilities of the device
            auto& ci = twsource.get_capability_interface();

            // Get the current unit of measure 
            auto vUnits = ci.get_units(capability_interface::get_current());
            if ( vUnits.empty() )
            { 
                std::cout << "Could not obtain the unit of measure for device \"" << 
                    twsource.get_source_info().get_product_name() << "\"\n";
                return 0;
            }

            // Get the x-resolution and y-resolution values supported by the device for the
            // specified unit of measure
            auto vxValues = ci.get_xresolution();
            auto vyValues = ci.get_yresolution();

            // If the values returned are in a range, let's expand the range.
            dynarithmic::twain::twain_range<capability_type::xresolution_type> testRangeX(vxValues);
            if (testRangeX.is_valid())
                vxValues = testRangeX.expand_range();

            // If the values returned are in a range, let's expand the range.
            dynarithmic::twain::twain_range<capability_type::yresolution_type> testRangeY(vyValues);
            if (testRangeY.is_valid())
                vyValues = testRangeY.expand_range();

            // print the results
            PrintResolutionValues(vxValues, vUnits.front(), 'X');
            PrintResolutionValues(vyValues, vUnits.front(), 'Y');

            // Now set one of the x-resolution values
            ci.set_xresolution({ vxValues[0] }); // sets the resolution to whatever the first value is.

            // see if there is an error
            auto ret = ci.get_last_error();
            if (!ret.return_value)
            {
                std::cout << "Setting the x-resolution failed.  Error is " <<
                    twain_session::get_error_string(ret.error_code);
            }
            else
            {
                std::cout << "Setting the x-resolution worked!";
                // Now get the current x-resolution to see if it matches up
                auto vCurrentX = ci.get_xresolution(ci.get_current());
                if ( vCurrentX.front() == vxValues[0] )
                    std::cout << "Getting the first set x-resolution worked!";
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
