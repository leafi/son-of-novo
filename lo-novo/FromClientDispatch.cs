using System;
using lo_novo.Protocol;

namespace lo_novo
{
    public static class FromClientDispatch
    {
        public static void QueryRoomContents(Room r)
        {
            State.Player.Comms.Send(FromServer.RoomCmdlet(r, "setContents " + string.Join(" ", r.AllContents.ConvertAll<string>((th) => th.Name.Replace(" ", "_")).ToArray())));
        }
    }
}

