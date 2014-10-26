using System;
using System.Net;
using System.Net.Sockets;

namespace lo_novo.Protocol
{
    public class StupidLoopbackClient : IComms
    {
        TcpClient tclient;

        public StupidLoopbackClient()
        {
            tclient = new TcpClient();
            tclient.Connect(new IPEndPoint(IPAddress.Loopback, 7001));

        }

        public string TryRead()
        {
            if (tclient.Available > 0)
                return new System.IO.BinaryReader(tclient.GetStream()).ReadString();
            return null;
        }

        public void Send(string s)
        {
            var bw = new System.IO.BinaryWriter(tclient.GetStream());
            bw.Write(s);
            bw.Flush();
        }

    }
}

