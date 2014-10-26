using System;

namespace lo_novo.Protocol
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

            return ses;
        }

        public static void StartStupidServer()
        {
            var sms = new StupidMultiplayerServerSession();
            State.GlobalComms = sms.Global;
            var p = new Player();
            p.Name = "Player";
            p.Comms = sms.Player;
            p.DefaultInventoryResponses = new DefaultInventoryResponses(p);
            State.AllPlayers.Add(p);
            State.RebuildNameToPlayer();
            State.Ticking.Add(sms);
        }

        // TODO: args? (bind ip, port)
        public static void StartMultiplayer()
        {
        }
    }
}
