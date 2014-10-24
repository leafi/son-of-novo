using System;

namespace lo_novo
{
    // Tracks state & does things per turn. Attacking players and getting mouthy and shit.
    // This is from the npc's perspective. This object is kept on NPC.
    public class BattleAI
    {
        public NPC NPC;

        public BattleAI(NPC npc) { this.NPC = npc; }

        public void Next() { }
    }
}

