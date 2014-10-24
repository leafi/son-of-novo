using System;
using System.Collections.Generic;

namespace lo_novo.Damage
{
    public static class Attack
    {
        public static void Do(Intention i, IDamageModel mdl)
        {
            if (i.AttackType == AttackType.None)
            {
                State.o("You decide to punch.");
                i.AttackType = AttackType.Punch;
            }

            AttackType valid = mdl.GetValidAttacks();

            // intersection of desired attacks & valid attacks
            AttackType atav = i.AttackType & valid;

            var trait = State.Player.Traits;

            // intersection non-empty?
            if (atav != AttackType.None)
                State.o("You " + i.VerbString + " " + i.ActiveNoun.ToString() + ".");

            // ...or must we rewrite the move?
            if (atav == AttackType.None)
            {
                var s = new List<string>();
                foreach (var at in Enum.GetValues(typeof(AttackType)))
                    if (i.AttackType.HasFlag((AttackType)at) && (AttackType)at != AttackType.None)
                        s.Add(((AttackType)at).ToString());

                if (s.Count == 1)
                    State.o(s[0] + "ing doesn't seem appropriate here.");
                else
                    State.o("Neither " + string.Join(" nor ", s) + " seems appropriate here.");
                    
                // If high SkillWrestling, then try do manhandle
                if (trait[Traits.WeaponWrestling] >= 5 && valid.HasFlag(AttackType.Manhandle))
                    atav = AttackType.Manhandle;
                else
                {
                    // Roll die against player SkillObservant then MagicTenacity then MagicLuck

                    AttackType good = (AttackType) Enum.Parse(typeof(AttackType), valid.ToString());

                    if (State.d20(Traits.SkillObservant, 5) >= 5)
                    {
                        State.o("You decide instead to " + good.ToString() + ".");
                        atav = good;
                    }
                    else
                    {
                        if (State.d20Hidden(Traits.MagicTenacity) >= 10)
                        {
                            State.o("You " + valid.ToString() + " instead.");
                            atav = good;
                        }
                        else
                        {
                            if (State.d20Hidden(Traits.MagicLuck) >= 10)
                            {
                                State.o("You find yourself " + good + "ing instead.");
                                atav = good;
                            }
                        }
                    }
                }
            }

            // Still bad?
            if (atav == AttackType.None)
            {
                State.o("Your attack is ineffectual.");
                return;
            }

            // else...
            // TODO: CRITICAL => FUMBLE == YES
            mdl.HandleAttack(atav, i.Violence, i.PassiveNoun, i.Whimsy >= 5, false, i.Airborne >= 5);
        }
    }
}

