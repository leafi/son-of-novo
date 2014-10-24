using System;

namespace lo_novo.LabRaid
{
    public class ControlRoom : Room
    {
        public ControlRoom()
        {
            Name = "Control Room";
            AddExit(Direction.West, typeof(WestCorridorS));
            AddExit(Direction.East, typeof(EastCorridorS));
        }
    }
}

