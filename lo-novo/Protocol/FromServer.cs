using System;
using System.Collections.Generic;

namespace lo_novo.Protocol
{
    public enum FromServerVerb
    {
        GlobalCmdlet,
        RoomCmdlet,
        ThingCmdlet,
        Text,
        Debug
    }

    public static class FromServer
    {
        static Dictionary<FromServerVerb, Delegate> dispatchers = new Dictionary<FromServerVerb, Delegate>();

        public static void AddDispatch(FromServerVerb verb, Delegate dispatch)
        {
            if (dispatchers.ContainsKey(verb))
                throw new Exception("already called FromServer.AddDispatch for " + verb);

            dispatchers.Add(verb, dispatch);
        }

        private static void assert(bool cond, string fmtStr, params object[] fmtArgs)
        {
            if (!cond)
                throw new Exception("FromServer parse error: " + string.Format(fmtStr, fmtStr, fmtArgs));
        }

        private static string fromRoom(Room r)
        {
            State.dbg("fromRoom");
            return r.ClassName;
        }

        private static string fromThing(Thing th)
        {
            return th.Owner.ClassName + " " + th.Name;
        }

        private static string autos(params string[] args)
        {
            foreach (var a in args)
                assert(!a.Contains("`"), "arg '{0}' has a ` in it. this is the network delineation character. (full msg '{1}')", a, string.Join(" /// ", args));
            return string.Join("`", args);
        }

        private static string[] splitThing(string s)
        {
            var bits = s.Split(new char[] { ' ' }, 2);
            assert(bits.Length == 2, "need both room and thing name in '{0}'", s);
            return bits;
        }

        public static void ParseAndDispatch(string serialized)
        {
            Func<string, string> thingOwner = ((s) => splitThing(s)[0]);
            Func<string, string> thingName = ((s) => splitThing(s)[1]);

            var abits = serialized.Split('`');

            switch (abits[0])
            {
                case "!":
                    // GlobalCmdlet
                    assert(abits.Length == 2, "GlobalCmdlet {0} bad format", serialized);
                    ((Action<string>)dispatchers[FromServerVerb.GlobalCmdlet])(abits[1]);
                    break;

                case "!R":
                    // RoomCmdlet
                    assert(abits.Length == 3, "RoomCmdlet {0} bad format", serialized);
                    ((Action<string, string>)dispatchers[FromServerVerb.RoomCmdlet])(abits[1], abits[2]);
                    break;

                case "!T":
                    // ThingCmdlet
                    assert(abits.Length == 3, "ThingCmdlet {0} bad format", serialized);
                    ((Action<string, string, string>)dispatchers[FromServerVerb.ThingCmdlet])(abits[1], thingOwner(abits[2]), thingName(abits[2]));
                    break;

                case ".":
                    // Text
                    assert(abits.Length == 2, "Text {0} bad format", serialized);
                    ((Action<string>)dispatchers[FromServerVerb.Text])(abits[1]);
                    break;

                case ".dbg":
                    // Debug
                    assert(abits.Length == 2, "Debug {0} bad format", serialized);
                    ((Action<string>)dispatchers[FromServerVerb.Debug])(abits[1]);
                    break;

                default:
                    assert(false, "invalid message type {0} in {1}", abits[0]);
                    break;
            }
        }

        public static string GlobalCmdlet(string cmdlet)
        {
            return autos("!", cmdlet);
        }

        public static string RoomCmdlet(Room room, string cmdlet)
        {
            State.dbg("roomcmdlet");
            return autos("!R", fromRoom(room), cmdlet);
        }

        public static string ThingCmdlet(Thing th, string cmdlet)
        {
            return autos("!T", fromThing(th), cmdlet);
        }

        public static string Text(string s)
        {
            return autos(".", s.Replace("`", " (backtick) "));
        }

        public static string Debug(string s)
        {
            return autos(".dbg", s.Replace("`", " (backtick) "));
        }


    }
}

