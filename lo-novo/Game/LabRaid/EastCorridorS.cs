using System;

namespace lo_novo.LabRaid
{
    public class EastCorridorS : Room
    {
        public EastCorridorS()
        {
            Name = "East Corridor (S)";

            AddExit(Direction.North, typeof(EastCorridorN));
            AddExit(Direction.East, typeof(ShitGym), "gym", "gymnasium");
            AddExit(Direction.West, typeof(ControlRoom), "control room");
        }
    }
}

