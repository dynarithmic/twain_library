#ifndef DTWAIN_RAII_H
#define DTWAIN_RAII_H

namespace dynarithmic
{
    template <typename T, typename DestroyTraits>
    struct DTWAIN_RAII
    {
        T m_a;
        DTWAIN_RAII(T a = T()) : m_a(a) {}
        void SetObject(T a) { m_a = a; }
        void Disconnect() { m_a = T(); }
        ~DTWAIN_RAII() { DestroyTraits::Destroy(m_a); }
    };
}
#endif