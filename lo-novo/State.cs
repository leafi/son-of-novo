using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lo_novo
{
    // Who triggered this command? and other fun questions
    public static class State
    {
        // TODO: make items that aren't the canonical source of information read-only to room scripts
        // probs backing var + prop { get { ... } }, no set {}, manual hidden set method elsewhere

        public static Player Player;
        public static Room Room { get { return Player.Room; } }
        public static Dictionary<string, Player> NameToPlayer = new Dictionary<string, Player>();
        public static List<Player> AllPlayers = new List<Player>();
        public static IIRC AllIRC;
        public static List<ITick> Ticking = new List<ITick>();

        public static Random RNG = new Random();

        public static Dictionary<Type, Room> AllSharedRooms = new Dictionary<Type, Room>();

        /// <summary>
        /// global game time in seconds
        /// </summary>
        public static double Time;

        /// <summary>
        /// time passed since last Tick() call in seconds
        /// </summary>
        public static double DeltaTime;

        internal static bool TravellingAll = false;

        /// <summary>
        /// Rolls d20 with skill adjustment. Announces roll to channel.
        /// </summary>
        public static int d20(Traits trait, int target = -1)
        {
            // http://rpg.stackexchange.com/questions/15971/is-it-possible-to-produce-a-bowl-shaped-probability-curve-with-dice-rolls
            // CodexArcanum's answer (3rd one down), Exact Odds of N on Xd20 Take Lowest
            // using 3d20 version. crit fail is if final result is 7.

            if (Player.Traits[trait] < 0)
                return 0;

            // 3d20, take lowest
            var d1 = RNG.Next(20) + 1;
            var d2 = RNG.Next(20) + 1;
            var d3 = RNG.Next(20) + 1;

            var least = ((d1 < d2) ? d1 : d2);
            if (d3 < least)
                least = d3;

            var adjusted = least + Player.Traits[trait];

            // announce result
            var s = string.Format("(Roll({4}): 3d20 -> {0} {1} {2}. Take least, {3}.", d1, d2, d3, least, trait);
            s += string.Format(" +{0}({1}) -> {2}", trait, Player.Traits[trait], adjusted);
            if (target != -1)
                s += "vs " + target + ((adjusted >= target) ? " => Success." : " => Fail.");
            s += ")";

            State.o(s);

            return adjusted;
        }
            
        /// <summary>
        /// Rolls d20 with skill adjustment, NOT announcing the roll or result at all.
        /// </summary>
        public static int d20Hidden(Traits trait)
        {
            if (Player.Traits[trait] < 0)
                return 0;

            // 3d20, take lowest (see d20 function)
            var d1 = RNG.Next(20) + 1;
            var d2 = RNG.Next(20) + 1;
            var d3 = RNG.Next(20) + 1;

            var least = ((d1 < d2) ? d1 : d2);
            if (d3 < least)
                least = d3;

            var adjusted = least + Player.Traits[trait];

            return adjusted;
        }

        public static string Choose(IEnumerable<string> choices)
        {
            return choices.ElementAt(RNG.Next(choices.Count()));
        }

        public static void SystemMessage(string msg)
        {
            AllIRC.Send("SYSTEM: " + msg);
        }

        public static IEnumerable<Room> GetOccupiedRooms()
        {
            return AllPlayers.ConvertAll((p) => p.Room).Distinct();
        }

        public static Room GetRoomContainingThing(Thing t)
        {
            if (State.Room != null && State.Room.Contents.Contains(t))
                return State.Room;

            return GetOccupiedRooms().TakeWhile((r) => r.Contents.Contains(t)).Single();
        }

        public static Player GetPlayerContainingThing(Thing t)
        {
            if (State.Player != null && State.Player.Inventory.Contains(t))
                return State.Player;

            return AllPlayers.TakeWhile((p) => p.Inventory.Contains(t)).Single();
        }

        public static INoun GetContainerOfThing(Thing t)
        {
            if (State.Player != null && State.Player.Inventory.Contains(t))
                return State.Player;

            return ((INoun) GetRoomContainingThing(t)) ?? (INoun) GetPlayerContainingThing(t);
        }

        /// <summary>
        /// Announces the text to all players.
        /// </summary>
        /// <param name="output">Text to display to all players</param>
        public static void o(string output)
        {
            State.AllIRC.Send(output);
        }

        public static void RebuildNameToPlayer()
        {
            NameToPlayer.Clear();

            foreach (var p in AllPlayers)
                NameToPlayer.Add(p.Name, p);

            foreach (var p in AllPlayers)
                foreach (var s in p.Aliases)
                    if (!NameToPlayer.ContainsKey(s))
                        NameToPlayer.Add(s, p);
        }

        public static void Travel(Room destination)
        {
            var prev = Player.Room;
            if (prev != null)
                prev.Leave();

            Player.Room = destination;
            if (destination != null)
                destination.Enter();
        }

        public static void Travel(Type roomClass, bool instanced = false)
        {
            // hmm...
            if (!instanced)
            {
                if (!AllSharedRooms.ContainsKey(roomClass))
                    AllSharedRooms.Add(roomClass, (Room) roomClass.GetConstructor(new Type[] { }).Invoke(null));

                Travel(AllSharedRooms[roomClass]);
            }
            else
            {
                Travel((Room) roomClass.GetConstructor(new Type[] { }).Invoke(null));
            }
        }

        public static void TravelAll(Room destination)
        {
            var old = State.Player;
            State.TravellingAll = true;
            foreach (var p in AllPlayers)
            {
                State.Player = p;
                Travel(destination);
            }
            State.TravellingAll = false;
            State.Player = old;
        }

        public static void TravelAll(Type roomClass, bool instanced = false)
        {
            var old = State.Player;
            State.TravellingAll = true;
            foreach (var p in AllPlayers)
            {
                State.Player = p;
                Travel(roomClass, instanced);
            }
            State.TravellingAll = false;
            State.Player = old;
        }
    }
}
