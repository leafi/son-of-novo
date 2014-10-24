using System;

namespace lo_novo
{
    public class DefaultInventoryResponses : IHandleDispatch
    {
        protected Player player;

        public DefaultInventoryResponses(Player p) { player = p; }

        public virtual bool Dispatch(Intention i)
        {
            return false;
        }
    }
}

