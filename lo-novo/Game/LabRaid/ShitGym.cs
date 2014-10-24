using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lo_novo.LabRaid
{
    public class ShitGym : Room
    {
        public ShitGym()
        {
            Name = "Shit Gymnasium";
            AddExit(Direction.West, typeof(EastCorridorS), "corridor");
        }
    }
}
