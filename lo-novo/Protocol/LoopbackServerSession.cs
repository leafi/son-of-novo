using System;
using System.Collections.Generic;
using System.IO;

namespace lo_novo.Protocol
{
    public class LoopbackServerSession : ITick
    {
        public Queue<string> Inbox = new Queue<string>();
        public Queue<string> Outbox = new Queue<string>();

        public IComms Global;
        public IComms Player;

        private StreamWriter DF;

        public LoopbackServerSession()
        {
            //Global = new GlobalComms(this);
            Player = new PlayerComms(this);
            Global = Player;

            DF = new StreamWriter("c:\\users\\leaf\\desktop\\dbg.txt");
            DF.AutoFlush = true;
        }

        public class GlobalComms : IComms
        {
            LoopbackServerSession lss;

            public GlobalComms(LoopbackServerSession lss) { this.lss = lss; }

            public string TryRead() { throw new NotImplementedException(); }

            public void Send(string s)
            {
                lock (lss.Outbox)
                    lss.Outbox.Enqueue(s);
                lss.DF.WriteLine(s);
            }
        }

        public class PlayerComms : IComms
        {
            LoopbackServerSession lss;

            public PlayerComms(LoopbackServerSession lss) { this.lss = lss; }

            public string TryRead()
            {
                string s = null;
                lock (lss.Inbox)
                    if (lss.Inbox.Count > 0)
                        s = lss.Inbox.Dequeue();
                if (s != null)
                    lss.DF.WriteLine("<<<" + s);

                return s;
            }

            public void Send(string s)
            {
                lock (lss.Outbox)
                    lss.Outbox.Enqueue(s);
                lss.DF.WriteLine(s);
            }
        }

        // Nothing to do!
        public void Tick() { }

    }
}

