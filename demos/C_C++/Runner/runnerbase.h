#ifndef RUNNERBASE_H
#define RUNNERBASE_H
#include <dynarithmic/twain/twain_session.hpp>  // for dynarithmic::twain::twain_source

struct RunnerBase
{
    virtual int Run() = 0;

    static dynarithmic::twain::source_select_info SelectDialog(dynarithmic::twain::twain_session& session)
    {
        // Get a twain dialog to customize
        dynarithmic::twain::twain_select_dialog twain_dialog;

        // Customize the dialog
        twain_dialog.
            set_parent_window(nullptr).
            set_title("Select Source").
            set_flags({ dynarithmic::twain::twain_select_dialog::showcenterscreen,
                       dynarithmic::twain::twain_select_dialog::sortnames,
                       dynarithmic::twain::twain_select_dialog::topmostwindow });

        // Use it to select the source
        return session.select_source(dynarithmic::twain::select_usedialog(twain_dialog));
    }

    virtual ~RunnerBase()
    {
        if (!IsDebuggerPresent())
        {
            printf("\nPress Enter key to exit application...\n");
            char temp;
            std::cin.get(temp);
        }
    }
};
#endif
