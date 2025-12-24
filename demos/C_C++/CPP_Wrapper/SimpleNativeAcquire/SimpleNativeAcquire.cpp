// SimpleNativeAcquire.cpp : Defines the entry point for the console application.
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

void DisplayAcquisitionAndImageInfo(twain_array& images)
{
    // Handle the images with the convenient image_handler wrapper class.
    image_handler img_handler(std::move(twain_source::get_images(images)));

    // Setting the auto-destroy flag, the image_handler will automatically destroy the images when
    // "img_handler" goes out of scope.  If we want to handle the images ourselves then the twain_array 
    // contains all the data, and thus you will need to use the twain_array interface to access the image data.
    img_handler.set_auto_destroy(true);

    // Let user know how many times they hit the button to start scanning pages
    std::cout << "You probably hit the scan button " << img_handler.get_num_acquisitions() << " times...\n";

    // An acquisition represents one scanning session
    // Get the number of images for each acquisition or scan session
    for (size_t current_acquisition = 0; current_acquisition < img_handler.get_num_acquisitions(); ++current_acquisition)
    {
        std::cout << "\n  Acquisition " << current_acquisition + 1 << " has the following info:\n";
        std::cout << "       Number of images acquired: " << img_handler.get_num_pages(current_acquisition) << "\n";
        std::cout << "       Handle to the image data:\n";
        for (size_t current_page = 0; current_page < img_handler.get_num_pages(current_acquisition); ++current_page)
        {
            /* Get the image data associated with acquisition i, page j */
            HANDLE hDib = img_handler(current_acquisition, current_page);
            if (hDib)
            {
                std::cout << "       Image " << current_page + 1 << ": " << hDib << "\n";
                SIZE_T imageDataSize = GlobalSize(hDib);
                std::cout << "       Image size in bytes: " << imageDataSize << "\n";

                // From here, we can call GlobalLock(hDib) to get the image data. 
                // Also for Windows, let's get a BMP from the DIB and write it out.

                // Uncomment the line of code below if you want to get the image data as a BMP...
                // std::vector<unsigned char> imagedata = img_handler.get_image_as_BMP(current_acquisition, current_page);
                //...
            }
        }
    }
}  // the images are automatically destroyed here


int Runner::Run()
{
    // Create a TWAIN session and automatically open the TWAIN data source manager
    twain_session session(startup_mode::autostart);

    // Now check if session was started successfully.  
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
            // set the characteristics to acquire in native mode.
            // Arrays of Device Independent Bitmap's will be returned for each page scanned
            twsource.get_acquire_characteristics().
                        get_general_options().
                           set_transfer_type(transfer_type::image_native);

            // Start the acquisition
            auto retval = twsource.acquire();

            // On return, retval.first will be the return code, retval.second is the container
            // of generated bitmap data

            // If there is an internal error, get the error
            if (twsource.acquire_internal_error(retval.first))
                std::cout << twain_session::get_error_string(twain_session::get_last_error());
            else
            // we scanned some images.    
            if (retval.first == twsource.acquire_ok)
                DisplayAcquisitionAndImageInfo(retval.second);
            else
                // user canceled
                std::cout << (retval.first == twsource.acquire_canceled ? "Canceled" : "Unknown status");
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
} // The twain_session ts will automatically close on exit of this function

int main()
{
    Runner().Run();
}