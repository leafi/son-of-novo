using System;

namespace lo_novo.LabRaid
{
    public class LivingQuartersB : Room
    {
        public LivingQuartersB()
        {
            Name = "Living Quarters Beta";

            AddExit(Direction.East, typeof(WestCorridorS), "corridor");
        }
    }
}

