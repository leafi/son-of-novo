using System;

namespace lo_novo
{
    // Tracks state & provides default responses for 'throw' and all the attack verbs.
    // This is from the player's perspective. This object is kept on Player.
    public class BattlePlayer
    {
        public Player Player;

        public BattlePlayer(Player player) { this.Player = player; }

        public void Dispatch(Intention intent)
        {
            //var target = intent.ActiveNoun as IBattleTarget;


        }

    }
}

