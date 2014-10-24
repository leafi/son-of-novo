using System;

namespace lo_novo
{
    public static class CommsSetup
    {
        public static LoopbackServerSession StartLoopback()
        {
            var ses = new LoopbackServerSession();
            State.GlobalComms = ses.Global;

            var p = new Player();
            p.Aliases.AddRange(new string[] { "player", "me", "protagonist", "self", "oneself", "myself" });
            p.Name = "Player";
            p.Comms = ses.Player;
            p.DefaultInventoryResponses = new DefaultInventoryResponses(p);

            State.AllPlayers.Add(p);

            State.RebuildNameToPlayer();

            State.Ticking.Add(ses);

            var op = State.Player;
            State.Player = p;
            State.Travel(typeof(LabRaid.WestMaintenance));
            State.Player = op;


            return ses;
        }

        // TODO: args? (bind ip, port)
        public static void StartMultiplayer()
        {
        }
    }
}
