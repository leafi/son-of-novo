using System;

namespace lo_novo.LabRaid
{
    public static class LabRaidState
    {
        public static int Oxygen = 100;
        public static bool SecurityActive { get; set; } // either property like this or add Ticker to poll...? meh
        public static bool MauveSyndrome = false;
        public static bool DoorsLocked = false;

        static LabRaidState()
        {
            // static constructors are a thing? you bet!
            SecurityActive = true;
        }
    }
}
