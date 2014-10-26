using System;

namespace lo_novo.Protocol
{
    public enum FromServerVerb
    {
        GlobalCmdlet,
        RoomCmdlet,
        ThingCmdlet,
        Text
    }



    public abstract class FromServer
    {
        public FromServer(string serialized)
        {
        }

        public static FromServer RoomCmdlet(Room room, string cmdlet)
        {
        }


    }
}

