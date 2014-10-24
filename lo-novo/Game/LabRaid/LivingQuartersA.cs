using System;

namespace lo_novo.LabRaid
{
    public class LivingQuartersA : Room
    {
        public LivingQuartersA()
        {
            Name = "Living Quarters Alpha";

            AddExit(Direction.East, typeof(WestCorridorN), "corridor");
        }
    }
}

