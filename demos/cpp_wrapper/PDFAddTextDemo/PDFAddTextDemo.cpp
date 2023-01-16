// PDFAddTextDemo.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <string>
#include <dynarithmic/twain/twain_session.hpp> // for dynarithmic::twain::twain_session
#include <dynarithmic/twain/twain_source.hpp>  // for dynarithmic::twain::twain_source
#include <dynarithmic/twain/pdf/pdf_text_element.hpp> // for pdf_text_element
#include <dynarithmic/twain/acquire_characteristics.hpp> // for acquire_characteristics

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

// Derive from twain_callback to trap any notifications sent by
// the TWAIN retrieval process
class pdf_callback_helper : public twain_callback
{
    pdf_text_element text_element;
    pdf_text_element text_element_2;
    int page_count = 1;

public:
    pdf_callback_helper()
    {
        // Set the defaults for the text element that we will use.
        text_element.
                set_font("Helvetica").
                set_position({ 100, 100 }).
                set_fontsize(14).
                set_scaling(100).
                set_color(RGB(255, 0, 0)).
                set_whichpages(pdf_printpage_value::currentpage);
        text_element_2.set_font("Courier-Bold").
                set_position({ 165, 350 }).
                set_fontsize(20).
                set_scaling(100).
                set_color(RGB(0, 255, 0)).
                set_text("Dynarithmic Software").
                set_whichpages(pdf_printpage_value::currentpage);
    }

    // Called when the filepagesaving notification is invoked
    int filepagesaving(twain_source& source) override
    {
        // The lower left portion of the page will have "Page x",
        // where "x" is the page number.
        text_element.set_text("Page " + std::to_string(page_count));

        // Write the PDF info
        text_element.add_text(source);
        text_element_2.add_text(source);
        ++page_count;
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
            // output the source product name
            std::cout << twsource.get_source_info().get_product_name() << "\n";

            // register our callback
            session.register_callback(twsource, pdf_callback_helper());

            // set the characteristics to acquire to a file.
            // This will acquire to a multi-page PDF file
            auto& ac = twsource.get_acquire_characteristics();
            ac.get_file_transfer_options().
                    set_type(filetype_value::pdfmulti).
                    set_name("PDFAddTextDemo.pdf");  // Name of the file

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