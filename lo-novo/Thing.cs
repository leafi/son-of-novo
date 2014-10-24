using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lo_novo
{
    public class Thing : INoun, IHandleDispatch
    {
        public string Name = "ENONAME";
        public string Preposition = "a";
        public string QuickDescription = null;
        public string Description = null;

        public List<string> AliasesRegex = new List<string>();

        public bool Heavy = true;
        public bool CanTake = false;

        protected Dictionary<DefaultVerb, FunOrString> actions = new Dictionary<DefaultVerb, FunOrString>();

        /// <summary>
        /// Should we tell the user about this object when describing the room?
        /// </summary>
        public bool Announce = true;

        public void AddAliases(params string[] aliasesRegex)
        {
            AliasesRegex.AddRange(aliasesRegex);
        }

        public FunOrString this[DefaultVerb dv]
        {
            get { return Get(dv); }
            set { Set(dv, value); }
        }
            
        public void Set(DefaultVerb verb, FunOrString funStr)
        {
            if (actions.ContainsKey(verb))
                actions.Remove(verb);

            if (funStr != null)
                actions.Add(verb, funStr);
        }

        public FunOrString Get(DefaultVerb verb)
        {
            return actions.ContainsKey(verb) ? actions[verb] : null;
        }

        public Thing() { }

        public Thing(string name, string description = null,
            FunOrString activate = null, FunOrString attack = null,
            FunOrString pushPull = null, FunOrString talk = null, FunOrString take = null, 
            FunOrString punt = null, FunOrString stop = null, FunOrString openClose = null, 
            FunOrString climbDescend = null, FunOrString modify = null)
        {
            this.Name = name;
            this.Description = description;

            if (activate != null)
                Set(DefaultVerb.Activate, activate);
            if (attack != null)
                Set(DefaultVerb.Attack, attack);
            if (pushPull != null)
            {
                Set(DefaultVerb.Push, pushPull);
                Set(DefaultVerb.Pull, pushPull);
            }
            if (talk != null)
                Set(DefaultVerb.Talk, talk);
            if (take != null)
                Set(DefaultVerb.Take, take);
            if (punt != null)
                Set(DefaultVerb.Punt, punt);
            if (stop != null)
                Set(DefaultVerb.Stop, stop);
            if (openClose != null)
            {
                Set(DefaultVerb.Open, openClose);
                Set(DefaultVerb.Close, openClose);
            }
            if (climbDescend != null)
            {
                Set(DefaultVerb.Climb, climbDescend);
                Set(DefaultVerb.Descend, climbDescend);
            }
            if (modify != null)
                Set(DefaultVerb.Modify, modify);
        }

        private bool magic(string outputOrNull)
        {
            if (outputOrNull != null)
            {
                State.o(outputOrNull);
                return true;
            }
            return false;
        }

        public virtual bool Dispatch(Intention i)
        {
            if (actions.ContainsKey(i.DefaultVerb))
            {
                var a = actions[i.DefaultVerb];

                if (a.IsString)
                    State.o(a);
                else if (((Func<Intention, bool>)a)(i))
                    return true;
            }

            if (i.DefaultVerb == DefaultVerb.Look)
            {
                State.o(Description ?? "ENODESCRIPTION");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Chance for Thing to do something in response to an action occurring involving it.
        /// </summary>
        /// <returns><c>true</c> if we did something that means the game should not adlib about what happened to the Thing, <c>false</c> otherwise.</returns>
        /// <param name="i">The index.</param>
        public virtual bool PostDispatch(Intention i)
        {
            return false;
        }

        public override string ToString()
        {
            return Preposition + " " + Name;
        }
       
    }
}
