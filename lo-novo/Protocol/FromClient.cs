using System;

namespace lo_novo.Protocol
{
    public enum FromClientVerb
    {
        QueryRoomContents
    }

    //
    // XXX: TODO: don't crash when receiving bad data from client
    // (this isn't a problem during development so much b/c we care about the crash info)
    //

    public static class FromClient
    {
        private static void assert(bool cond, string fmtStr, params object[] fmtArgs)
        {
            if (!cond)
                throw new Exception("FromClient parse error: " + string.Format(fmtStr, fmtStr, fmtArgs));
        }

        private static Room toRoom(string s)
        {
            assert(!s.Contains(" "), "room id contains string");
            var t = Type.GetType(s);
            assert(t != null, "couldn't find type {0} (should be room)", s);
            assert(t.IsSubclassOf(typeof(Room)), "type {0} isn't room", s);
            return State.AllSharedRooms[t];
        }

        private static Thing toThing(string s)
        {
            var bits = s.Split(new char[] { ' ' }, 2);
            assert(bits.Length == 2, "need both room and thing name in '{0}'", s);
            var room = toRoom(bits[0]);
            foreach (var th in room.AllContents)
                if (th.Name == bits[1])
                    return th;
            assert(false, "failed to find thing {0} in room {1} (msg {2})", bits[1], bits[0], bits[2]);
            return null; // unreachable
        }

        private static string autos(params string[] args)
        {
            foreach (var a in args)
                assert(!a.Contains("`"), "arg '{0}' has a ` in it. this is the network delineation character. (full msg '{1}')", a, string.Join(" /// ", args));
            return string.Join("`", args);
        }


        public static void ParseAndDispatch(string serialized)
        {
            var abits = serialized.Split('`');

            switch (abits[0])
            {
                case "?R":
                    // QueryRoomContents
                    assert(abits.Length == 2, "QueryRoomContents bad format {0}", serialized);
                    FromClientDispatch.QueryRoomContents(toRoom(abits[1]));
                    break;

                default:
                    // XXX: !!!!! HACK !!!!!!
                    //assert(false, "invalid message type {0} in {1}", abits[0], serialized);
                    State.Room.Parse(serialized);
                    break;
            }
        }

        public static string QueryRoomContents(string roomName)
        {
            return autos("?R", roomName);
        }
    }
}

