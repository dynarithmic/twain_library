#include <iostream>
#include <string>
#include <dynarithmic/twain/twain_session.hpp> // for dynarithmic::twain::twain_session
#include <dynarithmic/twain/twain_source.hpp>  // for dynarithmic::twain::twain_source

using namespace dynarithmic::twain;

struct Runner
{
    int Run(const char *sourceName);
    ~Runner()
    {
        printf("\nPress Enter key to exit application...\n");
        char temp;
        std::cin.get(temp);
    }
};

void ShowResults(twain_source& source, const char *ptrInfo)
{
    std::cout << ptrInfo;
    if (source.is_open())
    {
        std::cout << "The source \"" << source.get_source_info().get_product_name() << 
                    "\" was successfully opened!\n";
        source.close();
    }
    else
        std::cout << "Unable to open the source: \"" << source.get_source_info().get_product_name() << "\n";
}

void PlainSelectSource(twain_session& session)
{
    twain_source theSource = session.select_source();
    ShowResults(theSource, "\n (TWAIN Select Source dialog used) \n");
}

void CustomSelectSource(twain_session& session, const char *title)
{
    // Get a twain dialog to customize
    twain_select_dialog twain_dialog;

    // Customize the dialog
    twain_dialog.
        set_parent_window(nullptr).
        set_title(title).
        set_flags({twain_select_dialog::showcenterscreen, 
                   twain_select_dialog::sortnames,
                   twain_select_dialog::topmostwindow });

    // Use it to select the source
    twain_source theSource = session.select_source(select_usedialog(twain_dialog));

    ShowResults(theSource, "\n (TWAIN Customized Select Source dialog used) \n");
}

void DefaultSelectSource(twain_session& session)
{
    twain_source theSource = session.select_source(select_default());
    ShowResults(theSource, "\n (Default source was opened) \n");
}

void NamedSelectSource(twain_session& session, const char *sourceName)
{
    if (sourceName)
    {
        twain_source theSource = session.select_source(select_byname(sourceName));
        ShowResults(theSource, "\n (TWAIN Source selected by name) \n");
    }
}

void FrenchSelectSource(twain_session& session)
{
    bool bOk = session.set_language_resource("french");
    if (!bOk)
       std::cout << "Could not load French language resource\n";
    else
    {
        CustomSelectSource(session, "French dialog");
        session.set_language_resource("english");
    }
}

int Runner::Run(const char *sourceName)
{
    // Create a TWAIN session and automatically open the TWAIN data source manager
    twain_session session(startup_mode::autostart);

    // Now check if session was started successfully.  
    if (session)
    {
        PlainSelectSource(session);
        CustomSelectSource(session, "Custom dialog");
        DefaultSelectSource(session);
        NamedSelectSource(session, sourceName);
        FrenchSelectSource(session);
    }
    else
    {
        // Did not load the DSM.  Cannot start TWAIN system, let's get the error string
        std::cout << twain_session::get_error_string(twain_session::get_last_error());
    }
    return 0;
}

int main(int argc, char *argv[])
{
    if (argc > 1)
        Runner().Run(argv[1]);  // The argument is the product name of the source to open
    else
        Runner().Run(nullptr);
}
