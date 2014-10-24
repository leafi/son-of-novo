using System;

namespace lo_novo.LabRaid
{
    public class WestCorridorS : Room
    {
        public WestCorridorS()
        {
            Name = "West Corridor (S)";

            AddExit(Direction.NorthWest, typeof(LivingQuartersB), "quarters");
            AddExit(Direction.North, typeof(WestCorridorN));
            AddExit(Direction.SouthWest, typeof(WestMaintenance), "maintenance", "cupboard", "closet");
            AddExit(Direction.East, typeof(ControlRoom), "control");
        }
    }
}

