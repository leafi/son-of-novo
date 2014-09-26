﻿using System;

namespace lo_novo.LabRaid
{
    public class WestCorridorN : Room
    {
        #region implemented abstract members of Room

        public override string Name { get { return "West Corridor (N)"; } }

        public override string Description
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string ShortDescription
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        public WestCorridorN()
        {
            AddExit(Direction.West, typeof(LivingQuartersA), "quarters");
            AddExit(Direction.East, typeof(ProjectChamber), "project", "chamber");
            AddExit(Direction.South, typeof(WestCorridorS));
        }
    }
}
