using System;

namespace lo_novo
{
    public interface IComms
    {
        string TryRead();
        void Send(string s);
    }
}
