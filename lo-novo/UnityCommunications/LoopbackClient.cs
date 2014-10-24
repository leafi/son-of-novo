using System;

namespace lo_novo
{
    public class LoopbackClient : IComms
    {
        private LoopbackServerSession lss;

        public LoopbackClient(LoopbackServerSession lss)
        {
            this.lss = lss;
        }
            
        public string TryRead()
        {
            lock (lss.Outbox)
                if (lss.Outbox.Count > 1)
                    return lss.Outbox.Dequeue();
            return null;
        }

        public void Send(string s)
        {
            lock (lss.Inbox)
                lss.Inbox.Enqueue(s);
        }
            
    }
}

