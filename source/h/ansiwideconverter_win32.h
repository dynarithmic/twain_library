class ConvertW2A
{
    CTL_String m_sz;
    UINT nConvertCodePage;

public:
    ConvertW2A(LPCWSTR psz) : nConvertCodePage(CP_THREAD_ACP)
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
        if (psz == nullptr)
            return;
        int nLengthW = static_cast<int>(wcslen(psz)) + 1;
        int nLengthA = nLengthW * 4;
        std::vector<char> szBuffer(nLengthA);
        bool bFailed = (0 == ::WideCharToMultiByte(nConvertCodePage, 0, psz, nLengthW, szBuffer.data(), nLengthA, NULL, NULL)) ? true : false;
        if (bFailed)
        {
            if (GetLastError() == ERROR_INSUFFICIENT_BUFFER)
            {
                nLengthA = ::WideCharToMultiByte(nConvertCodePage, 0, psz, nLengthW, NULL, 0, NULL, NULL);
                szBuffer.resize(nLengthA);
                bFailed = (0 == ::WideCharToMultiByte(nConvertCodePage, 0, psz, nLengthW, szBuffer.data(), nLengthA, NULL, NULL)) ? true : false;
            }
        }
        if (bFailed)
            return;
        m_sz = CTL_String(szBuffer.data(), szBuffer.size());
    }
};

class ConvertA2W
{
    CTL_WString m_sz;
    UINT nConvertCodePage;

public:
    ConvertA2W(LPCSTR psz) : nConvertCodePage(CP_THREAD_ACP)
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
        if (psz == NULL)
            return;
        int nLengthA = static_cast<int>(strlen(psz)) + 1;
        int nLengthW = nLengthA;
        std::vector<wchar_t> szBuffer(nLengthW);
        bool bFailed = (0 == ::MultiByteToWideChar(nConvertCodePage, 0, psz, nLengthA, szBuffer.data(), nLengthW)) ? true : false;
        if (bFailed)
        {
            if (GetLastError() == ERROR_INSUFFICIENT_BUFFER)
            {
                nLengthW = ::MultiByteToWideChar(nConvertCodePage, 0, psz, nLengthA, NULL, 0);
                szBuffer.resize(nLengthW);
                bFailed = (0 == ::MultiByteToWideChar(nConvertCodePage, 0, psz, nLengthA, szBuffer.data(), nLengthW)) ? true : false;
            }
        }
        if (bFailed)
            return;
        m_sz = CTL_WString(szBuffer.data(), szBuffer.size());
    }
};
