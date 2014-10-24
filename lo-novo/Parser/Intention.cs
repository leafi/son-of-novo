using System;
using System.Text.RegularExpressions;
using lo_novo.Damage;

namespace lo_novo
{
    public class Intention
    {
        // all the following stats fields basically have 4 possible values:
        // 0, 1, 5 or 10. 0 is 'not at all', 1 is 'not especially', 10 is 'very much so'
        public int Violence = 1;
        public int Airborne = 1;
        public int Whimsy = 1;

        // meta
        public AttackType AttackType = AttackType.None;

        public DefaultVerb DefaultVerb = DefaultVerb.DontKnow;
        public string VerbString = "";

        public INoun ActiveNoun = null;
        public string ActiveNounString = "";
        public INoun PassiveNoun = null;
        public string PassiveNounString = "";

        public MatchCollection RegexMatches = null;

        /// <summary>
        /// Is this about nothing/the whole room, rather than a specific item or person?
        /// </summary>
        public bool WholeRoom { get { return ActiveNoun == null || ActiveNoun == State.Room; } }

    }
}
