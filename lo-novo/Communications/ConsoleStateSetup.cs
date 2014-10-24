using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lo_novo
{
    public static class ConsoleStateSetup
    {
        public static void Setup()
        {
            var irc = new Communications.ConsoleIRC();
            State.AllIRC = irc.SystemIRC;

            var p = new Player();
            p.Aliases.AddRange(new string[] { "player", "me", "protagonist", "self", "oneself", "myself" });
            p.Name = "Player";
            p.IRC = irc.PlayerIRC;
            p.DefaultInventoryResponses = new DefaultInventoryResponses(p);

            State.AllPlayers.Add(p);

            State.RebuildNameToPlayer();

            State.Ticking.Add(irc);
        }
    }
}
