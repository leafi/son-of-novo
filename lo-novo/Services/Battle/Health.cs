using System;

namespace lo_novo
{
    // TODO: ???
    public class Health
    {
        public int HP = 100;

        public bool Suffering { get { return HP < 21; } }
        public bool Dying { get { return HP < 1; } }
    }
}
