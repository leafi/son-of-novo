using System;

namespace lo_novo.LabRaid
{
    public class Landing2 : Room
    {
        public Landing2()
        {
            Name = "Landing #2";

            AddExit(Direction.South, typeof(ProjectChamber), "chamber", "project");
        }
    }
}

