using System;

namespace lo_novo.LabRaid
{
    public class WestCorridorN : Room
    {
        public WestCorridorN()
        {
            Name = "West Corridor (N)";

            AddExit(Direction.West, typeof(LivingQuartersA), "quarters");
            AddExit(Direction.East, typeof(ProjectChamber), "project", "chamber");
            AddExit(Direction.South, typeof(WestCorridorS));
        }
    }
}

