using System;

namespace lo_novo.PommeDeTerre
{
    public class EmergencyLights : Thing
    {
        public EmergencyLights(Room owner) : base(owner)
        {
            Name = "emergency lights";
        }
    }
}

