using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace lo_novo
{
    public static class IRCStateSetup
    {
        public static void Setup()
        {
            var irc = new IRCSession();

            while (irc.SystemIRC == null)
                Thread.Sleep(100);

            State.AllIRC = irc.SystemIRC;
            State.RebuildNameToPlayer();
            State.Ticking.Add(irc);
        }
    }
}
