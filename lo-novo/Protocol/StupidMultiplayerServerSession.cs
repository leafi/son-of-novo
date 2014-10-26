using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace lo_novo.Protocol
{
    public class StupidMultiplayerServerSession : ITick
    {
        public Queue<string> Inbox = new Queue<string>();
        public Queue<string> Outbox = new Queue<string>();

        public IComms Global;
        public IComms Player;

        private TcpListener tcpListener;
        private TcpClient tclient = null;

        public StupidMultiplayerServerSession()
        {
            //Global = new GlobalComms(this);
            Player = new PlayerComms(this);
            Global = Player;
            tcpListener = new TcpListener(IPAddress.Loopback, 7001);
            tcpListener.Start();
        }

        public class PlayerComms : IComms
        {
            StupidMultiplayerServerSession lss;

            public PlayerComms(StupidMultiplayerServerSession lss) { this.lss = lss; }

            public string TryRead()
            {
                string s = null;
                lock (lss.Inbox)
                    if (lss.Inbox.Count > 0)
                        s = lss.Inbox.Dequeue();
                if (s != null)
                    Console.WriteLine("<<<" + s);

                return s;
            }

            public void Send(string s)
            {
                lock (lss.Outbox)
                    lss.Outbox.Enqueue(s);
                Console.WriteLine(s);
            }
        }

        // Nothing to do!
        public void Tick()
        {
            if (tcpListener.Pending())
            {
                Console.WriteLine("Accepting client...");
                tclient = tcpListener.AcceptTcpClient();
            }

            if (tclient != null)
            {
                if (!tclient.Connected)
                {
                    Console.WriteLine("Disconnected.");
                }
                else
                {
                    if (tclient.Available > 0)
                    {
                        var s = new System.IO.BinaryReader(tclient.GetStream()).ReadString();
                        lock (Inbox)
                            Inbox.Enqueue(s);
                    }
                    lock (Outbox)
                        while (Outbox.Count > 0)
                        {
                            var bw = new System.IO.BinaryWriter(tclient.GetStream());
                            bw.Write(Outbox.Dequeue());
                            bw.Flush();
                        }
                }
            }
        }

    }
}

