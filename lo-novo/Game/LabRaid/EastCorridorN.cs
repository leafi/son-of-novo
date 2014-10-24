using System;

namespace lo_novo.LabRaid
{
    public class EastCorridorN : Room
    {
        public EastCorridorN()
        {
            Name = "East Corridor (N)";

            AddExit(Direction.West, typeof(ProjectChamber), "chamber");
            AddExit(Direction.NorthEast, typeof(DirectorsOffice), "office");
            AddExit(Direction.SouthEast, typeof(EastMaintenance), "maintenance", "cupboard", "closet");
            AddExit(Direction.South, typeof(EastCorridorS));
        }
    }
}

