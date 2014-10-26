using System;

namespace lo_novo.Protocol
{
    public interface IComms
    {
        string TryRead();
        void Send(string s);
    }
}
