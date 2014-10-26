using System;
using System.Collections.Generic;

namespace lo_novo.Protocol
{
    public class LoopbackServerSession : ITick
    {
        public Queue<string> Inbox = new Queue<string>();
        public Queue<string> Outbox = new Queue<string>();

        public GlobalComms Global;
        public PlayerComms Player;

        public LoopbackServerSession()
        {
            Global = new GlobalComms(this);
            Player = new PlayerComms(this);
        }

        public class GlobalComms : IComms
        {
            LoopbackServerSession lss;

            public GlobalComms(LoopbackServerSession lss) { this.lss = lss; }

            public string TryRead() { throw new NotImplementedException(); }

            public void Send(string s)
            {
                lock (lss.Outbox)
                    foreach (var line in s.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        lss.Outbox.Enqueue(line);
            }
        }

        public class PlayerComms : IComms
        {
            LoopbackServerSession lss;

            public PlayerComms(LoopbackServerSession lss) { this.lss = lss; }

            public string TryRead()
            {
                lock (lss.Inbox)
                    if (lss.Inbox.Count > 0)
                        return lss.Inbox.Dequeue();
                return null;
            }

            public void Send(string s)
            {
                lock (lss.Outbox)
                    foreach (var line in s.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        lss.Outbox.Enqueue(line);
            }
        }

        // Nothing to do!
        public void Tick() { }

    }
}

