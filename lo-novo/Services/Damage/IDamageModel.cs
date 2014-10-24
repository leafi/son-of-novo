using System;

namespace lo_novo.Damage
{
    public interface IDamageModel
    {
        AttackType GetValidAttacks();
        void HandleAttack(AttackType at, int vio, INoun with = null, bool whi = false, bool critFail = false, bool up = false);
    }
}

