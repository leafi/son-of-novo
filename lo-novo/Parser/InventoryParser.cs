using System;
using System.Collections.Generic;

namespace lo_novo
{
    /// <summary>
    /// System commands related to player inventory.
    /// </summary>
    public static class InventoryParser
    {
        public static bool Parse(string verb, IEnumerable<string> all)
        {
            switch (verb)
            {
                case "i":
                case "inv":
                case "inventory":
                case "bag":
                    State.Player.IRC.Send("Let's see what's in your bag... (TODO)");

                    // ...

                    return true;

                default:
                    return false;
            }
        }
    }
}

