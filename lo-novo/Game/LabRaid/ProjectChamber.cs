using System;

namespace lo_novo.LabRaid
{
    public class ProjectChamber : Room
    {
        public ProjectChamber()
        {
            Name = "Project Chamber";

            AddExit(Direction.NorthWest, typeof(Landing1), "landing (#)?1", "(my|our) ship");
            AddExit(Direction.NorthEast, typeof(Landing2), "landing (#)?2", "secret");
            AddExit(Direction.West, typeof(WestCorridorN));
            AddExit(Direction.East, typeof(EastCorridorN));
        }
    }
}

