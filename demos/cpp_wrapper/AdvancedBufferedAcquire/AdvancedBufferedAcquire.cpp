#include <iostream>
#include <string>
#include <fstream>

#include <dynarithmic/twain/twain_session.hpp> // for dynarithmic::twain::twain_session
#include <dynarithmic/twain/twain_source.hpp>  // for dynarithmic::twain::twain_source
#include <dynarithmic/twain/acquire_characteristics.hpp>  // for acquire_characteristics
#include "..\Runner\runnerbase.h"

struct Runner : RunnerBase
{
    int Run();
};

using namespace dynarithmic::twain;


class buffered_callback : public twain_callback 
{
    int64_t m_bufferWritten = 0;
    std::ofstream* m_out;

    private:
        struct Locker
        {
            HANDLE m_h;
            Locker(HANDLE h) : m_h(h) {}
            ~Locker() { GlobalUnlock(m_h); }
        };

        void addstrip(twain_source& source, bool morestrips)
        {
            // Note that all we do is append the strips.  It is the responsibility
            // of the application to handle the data.
            ++m_bufferWritten;

            // Add the strip to the buffer 
            // First, get the number of bytes received (that's all we care about in the example) 
            auto& stripdata = source.get_buffered_transfer_info().get_strip_data();
            std::cout << "\nWriting buffer strip " << m_bufferWritten << ".  Number of bytes written will be "
                << stripdata.BytesWritten << "\n";

            // Get the memory that holds the strip of data.  For Windows, this will
            // always be in the form of a HANDLE that requires GlobalLock() to access
            // the bytes of data
            HANDLE hTheDibStrip = source.get_buffered_transfer_info().getstrip();

            // Lock the strip of data to a pointer */
            BYTE* pTheDibStrip = reinterpret_cast<BYTE*>(GlobalLock(hTheDibStrip));

            // Make sure we unlock when leaving this function
            Locker lock(hTheDibStrip);

            // Write the strip of data to the output file 
            if ( *m_out )
                m_out->write(reinterpret_cast<const char *>(pTheDibStrip), stripdata.BytesWritten);

            // We know it's the last strip if the transferdone virtual function is
            // fired.
            if (!morestrips)
                m_out->close();
        }

    public:
        void setFile(std::ofstream* pFile)
        {
            m_out = pFile;
        }

        int transferready(twain_source& source) override
        {
            image_information imageinfo = source.get_current_image_information();
            // The image_information will contain the information concerning the
            // image that will be acquired.  Use this to set up any image headers,
            // structures, etc.  
            return 1;
        }

        // Sent after a strip of image data has been transferred
        // from the device to the application
        int transferstripdone(twain_source& source) override
        {
            addstrip(source, true);
            return 1;
        }

        // Sent when the last strip of image data has been transferred
        // from the device to the application
        int transferdone(twain_source& source) override
        {
            addstrip(source, false);
            return 1;
        }
};

buffered_callback bufCallback;
std::ofstream rawFile;

void SetupMemoryBuffer(twain_source& source)
{
    // Set the buffered strip size
    auto& bufInfo = source.get_buffered_transfer_info();
    bufInfo.set_stripsize(bufInfo.preferredsize());

    // Create the output file that will consist of raw image data 
    rawFile.open("test.bin", std::ios::binary);
    bufCallback.setFile(&rawFile);
}

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
            return -1;
        }

        // open the source from the selection above    
        twain_source twsource(selection);

        // check if we were able to open the source
        if (twsource.is_open())
        {
            // Setup the memory buffer
            SetupMemoryBuffer(twsource);

            // register the buffered transfer callback 
            session.register_callback(twsource, bufCallback);

            // Set the compression option
            auto& compressOptions = twsource.get_acquire_characteristics().get_compression_options();

            // No compression for this demo, since all devices must support compression-less
            // buffered transfers.  To get a list of compression types that your device supports:
            // auto compressTypes = twsource.get_buffered_transfer_info().get_compression_types();
            compressOptions.set_compression(compression_value::none);

            // Now set the general options to acquire using the buffered mode
            auto& gOpts = twsource.get_acquire_characteristics().get_general_options();
            gOpts.set_transfer_type(transfer_type::image_buffered);

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

    return 0;
}


int main()
{
    Runner().Run();
}
