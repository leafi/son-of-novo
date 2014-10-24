using System;

namespace lo_novo.LabRaid
{
    public class DirectorsOffice : Room
    {
        public DirectorsOffice()
        {
            Name = "Director's Office";

            AddExit(Direction.West, typeof(EastCorridorN), "corridor");
        }
    }
}

