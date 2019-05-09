class ConvertW2A
{
    CTL_String m_sz;

public:
    ConvertW2A(LPCWSTR psz)
    {
        Init(psz);
    }

    operator LPCSTR() const
    {
        return(m_sz.c_str());
    }

private:
    void Init(LPCWSTR psz)
    {
        if (!psz)
            return;
        m_sz = std::move(CTL_String(psz, psz + wcslen(psz)));
    }
};

class ConvertA2W
{
    CTL_WString m_sz;

public:
    ConvertA2W(LPCSTR psz)
    {
        Init(psz);
    }

    operator LPCWSTR() const
    {
        return(m_sz.c_str());
    }

private:
    void Init(LPCSTR psz)
    {
        if (!psz)
            return;
        m_sz = std::move(CTL_WString(psz, psz + strlen(psz)));
    }
};
