#include <iostream>
#include <string>
#include <dynarithmic/twain/twain_session.hpp> // for dynarithmic::twain::twain_session
#include <dynarithmic/twain/twain_source.hpp>  // for dynarithmic::twain::twain_source

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
    using namespace dynarithmic::twain;

    // Create a TWAIN session 
    twain_session session;

    std::array<dsm_type, 2> dsm_to_test = { dsm_type::version2_dsm, dsm_type::legacy_dsm };

    for (int i = 0; i < 2; ++i)
    {
        // Set the TWAIN Data Source Manager to the version we
        // are using.
        // This must be done before starting a TWAIN session,
        // or if changed while a session has started, the session
        // must be restarted to reflect the change in the DSM being used.
        session.set_dsm(dsm_to_test[i]);

        // Start the session
        session.start();

        // Now check if session was started successfully.  
        if (session)
        {
            // Get the TWAIN information 
            std::cout << "TWAIN DSM Path in use: " << session.get_dsm_path() << "\n";

            // Let's create a local block
            {
                // select a source using the dialog for the version 2.0 TWAIN DSM
                twain_source theSource = session.select_source();
            } // The source will be closed automatically

            // Stop the session and choose the legacy DSM.
            // Note that this also sets the selected source to inactive.
            session.stop();
        }
        else
        {
            // Did not load the DSM.  Cannot start TWAIN system, let's get the error string
            std::cout << twain_session::get_error_string(twain_session::get_last_error()) << "\n";
        }
    }
    return 1;
}

int main()
{
    Runner().Run();
}