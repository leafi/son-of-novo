using System;

namespace lo_novo.LabRaid
{
    public class Landing1 : Room
    {
        public Landing1()
        {
            Name = "Landing #1";

            AddExit(Direction.South, typeof(ProjectChamber), "chamber", "project");
        }
    }
}

