#ifndef RUNNERBASE_H
#define RUNNERBASE_H
struct RunnerBase
{
    virtual int Run() = 0;
    ~RunnerBase()
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
