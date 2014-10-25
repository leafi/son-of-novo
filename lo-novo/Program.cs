using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace lo_novo
{
    public class Program
    {
        //public const bool Safe = false;

        public static bool Running = false;
        public static LoopbackClient LoopbackClient = null;


        public static void StartMultiplayer(string[] args)
        {
            throw new NotImplementedException();
        }

        public static LoopbackClient StartLoopbackAndJoin(string[] args)
        {
            var lc = new LoopbackClient(CommsSetup.StartLoopback());
            ForkMain(args);
            LoopbackClient = lc; // unity editor leaves our thread running when going Play->Stop game state
            return lc;
        }

        public static void JoinMultiplayer(string[] args)
        {
            throw new NotImplementedException();
        }

        public static void Stop()
        {
            Running = false;
        }


        private static void ForkMain(string[] args)
        {
            Running = true;
            Thread t = new Thread(new ThreadStart(Main));
            t.Start();
        }


        private static void Main(/*string[] args*/)
        {
            //IRCStateSetup.Setup();

            State.SystemMessage("Welcome, one and all. Let's start the game.\n");

            // move all players to init room
            foreach (var p in State.AllPlayers)
            {
                State.Player = p;
                State.Travel(typeof(LabRaid.WestMaintenance));
            }


            var toTick = new List<ITick>();
            var lastTime = DateTime.UtcNow;

            while (Running)
            {
                toTick.Clear();
                toTick.AddRange(State.Ticking);

                var newTime = DateTime.UtcNow;
                double dt = (newTime - lastTime).TotalSeconds;
                if (dt > 5.00)
                {
                    //State.SystemMessage("Lagging, or time changed? " + dt.ToString() + "s passed, which is >5s");
                    dt = 5.00;
                }
                State.Time += dt;
                State.DeltaTime = dt;
                lastTime = newTime;

                foreach (var t in toTick)
                    t.Tick();

                foreach (var p in State.AllPlayers)
                {
                    State.Player = p;

                    while (true)
                    {
                        var s = p.Comms.TryRead();
                        if (s != null)
                            p.Room.Parse(s);
                        else
                            break;
                    }
                }

                Thread.Sleep(10);
            }
            

        }
    }
}
