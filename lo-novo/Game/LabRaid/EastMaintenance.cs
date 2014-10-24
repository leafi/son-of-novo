using System;

namespace lo_novo.LabRaid
{
    public class EastMaintenance : Room
    {
        public EastMaintenance()
        {
            Name = "Maintenance Cupboard (E)";



            AddExit(Direction.West, typeof(EastCorridorN), "corridor");
        }
    }
}

