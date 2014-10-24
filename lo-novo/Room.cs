using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lo_novo
{
    public abstract class Room : IHandleDispatch, ITick, INoun
    {
        public string Name = null;
        public string Description = null;
        public virtual string[] Tags { get { return new string[] { }; } }
        public List<Thing> Contents = new List<Thing>();

        protected RoomParser parser;
        internal DefaultRoomResponses DefaultRoomResponses;

        public List<Tuple<string, EatsNoun, Func<Intention, bool>>> CustomRegex
            = new List<Tuple<string, EatsNoun, Func<Intention, bool>>>();

        // REGEX!!!
        public List<Tuple<string, Type>> Exits = new List<Tuple<string, Type>>();
        public List<string> ExitCanonicalNames = new List<string>();

        private string lastFullDescription = "";

        public enum Direction
        {
            None,
            North,
            East,
            South,
            West,
            NorthWest,
            NorthEast,
            SouthEast,
            SouthWest
        }

        public void AddExit(Direction direction, Type destination)
        {
            if (direction == Direction.None)
                throw new ArgumentException("Can't add exit with null direction without any aliases.");

            Exits.Add(Tuple.Create(Enum.GetName(typeof(Direction), direction).ToLowerInvariant(), destination));
            ExitCanonicalNames.Add(Enum.GetName(typeof(Direction), direction).ToUpper());
        }

        public void AddExit(Direction direction, Type destination, params string[] aliasesRegex)
        {
            if (direction == Direction.None)
                throw new ArgumentException("A canonical name must be provided for the exit, because direction is None.");

            var dn = Enum.GetName(typeof(Direction), direction);
            var ar = new string[] { dn }.Union(aliasesRegex);

            foreach (var r in ar)
                Exits.Add(Tuple.Create(r.ToLowerInvariant(), destination));
            ExitCanonicalNames.Add(dn.ToUpper());
        }

        public void AddExit(Direction direction, Type destination, bool changeCanonicalName, string canonicalName, params string[] aliasesRegex)
        {
            if (!changeCanonicalName) // fucking types
                throw new Exception();

            var add = (direction != Direction.None) ? new string[] { canonicalName, Enum.GetName(typeof(Direction), direction) } : new string[] { canonicalName };
            var ar = add.Union(aliasesRegex);

            foreach (var r in ar)
                Exits.Add(Tuple.Create(r.ToLowerInvariant(), destination));
            ExitCanonicalNames.Add(canonicalName.ToUpper());
        }

        public Thing Find(string name)
        {
            return Contents.Where<Thing>((t) => t.Name == name).Single();
        }

        public T Find<T>() where T : Thing
        {
            return Contents.Where<Thing>((t) => typeof(T) == t.GetType()).Cast<T>().Single();
        }

        public T Find<T>(string name) where T : Thing
        {
            return Contents.Where<Thing>((t) => typeof(T) == t.GetType()).Cast<T>()
                .Where<T>((p) => p.Name == name).Single();
        }

        public IEnumerable<Player> Players
        {
            get
            {
                return State.AllPlayers.Where<Player>((p) => p.Room == this);
            }
        }

        public Room()
        {
            BuildParser();
        }

        protected FunOrString Func(Func<Intention, bool> lambda) { return (FunOrString) lambda; }

        public virtual string GetFullDescription()
        {
            var lines = new List<string>();

            lines.Add((Name ?? "ENONAME").ToUpper());
            lines.Add(Description ?? "ENODESCRIPTION");

            var noQuickDesc = new List<string>();

            foreach (var t in Contents)
                if (t.Announce)
                {
                    if (t.QuickDescription != null)
                        lines.Add(t.QuickDescription);
                    else
                        noQuickDesc.Add(t.ToString());
                }

            if (noQuickDesc.Count > 0)
                lines.Add("Additionally, there's " + string.Join(", ", noQuickDesc) + ".");

            if (Exits.Count > 0)
                lines.Add("Exits lie to the " + string.Join(", ", ExitCanonicalNames) + ".");

            return string.Join("\n", lines);
        }

        public virtual void Enter()
        {
            if (!State.Ticking.Contains(this))
                State.Ticking.Add(this);

            var d = GetFullDescription();
            if (d != lastFullDescription)
            {
                lastFullDescription = d;
                State.o(d);
            }
            else if (!State.TravellingAll)
                State.o("-> " + (Name ?? "ENONAME"));
        }

        public virtual void Leave() 
        {
            if (State.Ticking.Contains(this))
                State.Ticking.Remove(this);
        }

        public virtual void Tick() { }

        protected virtual void BuildParser()
        {
            parser = new RoomParser(this);
            DefaultRoomResponses = new DefaultRoomResponses(this);
        }

        public virtual void Parse(string s)
        { 
            parser.Parse(s); 
        }

        /// <summary>
        /// Postprocessing function on parse result, letting the room script tweak the Intention if it wishes.
        /// </summary>
        /// <returns>same or new Intention, possibly with changed fields</returns>
        /// <param name="intent">Intention before post-processing</param>
        /// <param name="s">original string before parsing</param>
        public virtual Intention ParsePostprocess(Intention intent, string s) 
        {
            return intent;
        }

        /// <summary>
        /// Chance for a room to handle/change a command before traditional dispatching.
        /// </summary>
        /// <param name="i">the Intention about to undergo traditional dispatch</param>
        public virtual void PreDispatch(Intention i) { }

        public virtual bool Dispatch(Intention i)
        {
            return false;
        }

        public override string ToString()
        {
            return (State.Room == this) ? "the room" : (Name ?? "ENONAME");
        }

    }
}
