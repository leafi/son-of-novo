using System;
using System.Collections.Generic;
using lo_novo.Damage;

namespace lo_novo
{
    public static class VerbClassifier
    {
        private static void si(Intention i, DefaultVerb pv, int air, int violent, int whimsy)
        {
            // all the stats fields basically have 4 possible values:
            // 0, 1, 5 or 10. 0 is 'not at all', 1 is 'not especially', and 10 is 'very much so'.
            i.DefaultVerb = pv;
            i.Airborne = air;
            i.Violence = violent;
            i.Whimsy = whimsy;
        }

        public static Intention Verbify(string s)
        {
            var i = new Intention();
            i.VerbString = s.ToLower().Trim();
            var v = i.VerbString.Replace('-', ' ');

            // TODO: regexp!

            switch (v)
            {
                case "add":
                    // add to inventory, or add part?
                    si(i, DefaultVerb.Take, 1, 1, 1);
                    break;

                case "arrest":
                    si(i, DefaultVerb.Stop, 0, 5, 5);
                    i.AttackType = AttackType.Halt | AttackType.Manhandle;
                    break;

                case "halt":
                    si(i, DefaultVerb.Stop, 0, 1, 1);
                    i.AttackType = AttackType.Halt;
                    break;

                case "press":
                    si(i, DefaultVerb.Activate, 1, 1, 1);
                    i.AttackType = AttackType.Push;
                    break;

                case "activate":
                    si(i, DefaultVerb.Activate, 1, 1, 1);
                    break;

                case "flick":
                    si(i, DefaultVerb.Activate, 1, 1, 5);
                    i.AttackType = AttackType.Push;
                    break;

                case "flip":
                    si(i, DefaultVerb.Activate, 1, 1, 1);
                    i.AttackType = AttackType.Manhandle | AttackType.Push;
                    break;

                case "turn on":
                    si(i, DefaultVerb.Activate, 1, 1, 1);
                    i.AttackType = AttackType.Manhandle;
                    break;

                case "poke":
                    si(i, DefaultVerb.Push, 0, 1, 10);
                    i.AttackType = AttackType.Push;
                    break;

                case "prod":
                    si(i, DefaultVerb.Attack, 1, 1, 10);
                    i.AttackType = AttackType.Push;
                    break;

                case "kick":
                    si(i, DefaultVerb.Attack, 5, 5, 2);
                    i.AttackType = AttackType.Kick;
                    break;

                case "punch":
                    si(i, DefaultVerb.Attack, 1, 10, 1);
                    i.AttackType = AttackType.Punch;
                    break;

                case "uppercut":
                case "upper cut":
                case "falconpunch":
                case "falcon punch":
                    si(i, DefaultVerb.Attack, 10, 10, 3);
                    i.AttackType = AttackType.Punch | AttackType.Kick;
                    break;

                case "murder":
                    si(i, DefaultVerb.Attack, 0, 10, 5);
                    i.AttackType = AttackType.Destroy;
                    break;

                case "destroy":
                    si(i, DefaultVerb.Attack, 1, 10, 0);
                    i.AttackType = AttackType.Destroy;
                    break;

                case "headbutt":
                    si(i, DefaultVerb.Attack, 1, 5, 5);
                    i.AttackType = AttackType.Manhandle | AttackType.Push;
                    break;

                case "roundhouse":
                case "round house":
                    si(i, DefaultVerb.Attack, 1, 10, 3);
                    i.AttackType = AttackType.Kick | AttackType.Manhandle;
                    break;

                case "piledrive":
                case "piledriver":
                    si(i, DefaultVerb.Attack, 0, 10, 5);
                    i.AttackType = AttackType.Manhandle;
                    break;

                case "wrestle":
                    si(i, DefaultVerb.Attack, 1, 5, 1);
                    i.AttackType = AttackType.Manhandle;
                    break;

                case "assault":
                    si(i, DefaultVerb.Attack, 1, 10, 5);
                    i.AttackType = AttackType.Manhandle | AttackType.Punch | AttackType.Stab;
                    break;

                case "attack":
                    si(i, DefaultVerb.Attack, 1, 10, 1);
                    i.AttackType = AttackType.Punch | AttackType.Push | AttackType.Stab | AttackType.Kick | AttackType.Manhandle;
                    break;

                case "rip":
                    si(i, DefaultVerb.Attack, 5, 10, 10);
                    i.AttackType = AttackType.Tear;
                    break;

                case "pluck":
                    si(i, DefaultVerb.Attack, 1, 1, 10);
                    i.AttackType = AttackType.Tear;
                    break;

                case "rend":
                    si(i, DefaultVerb.Attack, 3, 10, 5);
                    i.AttackType = AttackType.Tear;
                    break;

                case "dislocate":
                    si(i, DefaultVerb.Attack, 1, 10, 10);
                    i.AttackType = AttackType.Tear | AttackType.Push;
                    break;

                case "strangle":
                    si(i, DefaultVerb.Attack, 1, 10, 3);
                    i.AttackType = AttackType.Manhandle | AttackType.Tear;
                    break;

                case "push":
                    si(i, DefaultVerb.Push, 1, 1, 1);
                    i.AttackType = AttackType.Push;
                    break;

                case "move":
                    si(i, DefaultVerb.Push, 1, 1, 1);
                    i.AttackType = AttackType.Push;
                    break;

                case "drive":
                    si(i, DefaultVerb.Push, 1, 1, 2);
                    i.AttackType = AttackType.Manhandle | AttackType.Push;
                    break;

                case "shove":
                    si(i, DefaultVerb.Push, 1, 5, 1);
                    i.AttackType = AttackType.Push;
                    break;

                case "nudge":
                    si(i, DefaultVerb.Push, 1, 1, 5);
                    i.AttackType = AttackType.Push;
                    break;

                case "exert force":
                    si(i, DefaultVerb.Push, 2, 5, 2);
                    i.AttackType = AttackType.Kick | AttackType.Push | AttackType.Manhandle | AttackType.Punch;
                    break;

                case "jolt":
                    si(i, DefaultVerb.Push, 1, 10, 2);
                    i.AttackType = AttackType.Tear | AttackType.Push;
                    break;

                case "propel":
                    si(i, DefaultVerb.Push, 5, 1, 5);
                    i.AttackType = AttackType.Push;
                    break;

                case "pull":
                    si(i, DefaultVerb.Pull, 1, 1, 1);
                    i.AttackType = AttackType.Push;
                    break;

                case "haul":
                    si(i, DefaultVerb.Pull, 5, 3, 3);
                    i.AttackType = AttackType.Push | AttackType.Manhandle;
                    break;

                case "drag":
                case "lug":
                case "heave":
                    si(i, DefaultVerb.Pull, 1, 5, 5);
                    i.AttackType = AttackType.Push | AttackType.Manhandle;
                    break;

                case "yank":
                    si(i, DefaultVerb.Pull, 3, 5, 10);
                    i.AttackType = AttackType.Push | AttackType.Tear;
                    break;

                case "tow":
                    si(i, DefaultVerb.Pull, 1, 1, 3);
                    i.AttackType = AttackType.Push;
                    break;

                case "chitchat":
                case "chit chat":
                case "natter":
                    si(i, DefaultVerb.Talk, 1, 1, 5);
                    break;

                case "ask":
                case "chat":
                case "discuss":
                case "talk":
                case "question":
                    si(i, DefaultVerb.Talk, 1, 1, 1);
                    break;

                case "converse":
                case "enquire":
                case "query":
                case "whisper":
                    si(i, DefaultVerb.Talk, 1, 1, 3);
                    break;
                
                case "shout":
                case "thunder":
                case "exclaim":
                case "rage":
                    si(i, DefaultVerb.Talk, 1, 5, 5);
                    break;

                case "insult":
                    si(i, DefaultVerb.Talk, 1, 10, 5);
                    break;

                case "take":
                case "pick up":
                case "pick":
                    si(i, DefaultVerb.Take, 1, 1, 1);
                    i.AttackType = AttackType.Manhandle;
                    break;

                case "grab":
                case "yoink":
                case "steal":
                case "wrench":
                    si(i, DefaultVerb.Take, 1, 10, 5);
                    i.AttackType = AttackType.Tear | AttackType.Push | AttackType.Manhandle;
                    break;


                // OK BORED NOW
                case "throw":
                    si(i, DefaultVerb.Punt, 10, 5, 1);
                    i.AttackType = AttackType.Manhandle | AttackType.Push;
                    break;

                case "punt":
                    si(i, DefaultVerb.Punt, 5, 5, 10);
                    i.AttackType = AttackType.Kick | AttackType.Push;
                    break;

                case "stop":
                    si(i, DefaultVerb.Stop, 0, 2, 1);
                    i.AttackType = AttackType.Halt;
                    break;

                case "open":
                    si(i, DefaultVerb.Open, 1, 1, 1);
                    i.AttackType = AttackType.Tear;
                    break;

                case "close":
                case "shut":
                    si(i, DefaultVerb.Close, 1, 1, 1);
                    i.AttackType = AttackType.Push;
                    break;

                case "climb up":
                case "climb":
                case "up":
                case "go up":
                    si(i, DefaultVerb.Climb, 1, 1, 1);
                    break;

                case "clamber up":
                case "clamber":
                    si(i, DefaultVerb.Climb, 2, 1, 5);
                    i.AttackType = AttackType.Manhandle;
                    break;

                case "climb down":
                case "descend":
                case "down":
                case "go down":
                    si(i, DefaultVerb.Descend, 1, 1, 1);
                    i.AttackType = AttackType.Push;
                    break;

                case "clamber down":
                    si(i, DefaultVerb.Descend, 2, 1, 5);
                    i.AttackType = AttackType.Push;
                    break;

                case "modify":
                    si(i, DefaultVerb.Modify, 1, 1, 1);
                    i.AttackType = AttackType.Manhandle | AttackType.Tear;
                    break;

                case "repair":
                case "fix":
                    si(i, DefaultVerb.Modify, 1, 1, 1);
                    break;

                case "look":
                case "examine":
                case "peer":
                case "take a closer look":
                case "x":
                    si(i, DefaultVerb.Look, 1, 1, 1);
                    break;

                case "what":
                case "what is":
                case "what is a":
                case "define a":
                case "define the":
                case "define":
                    si(i, DefaultVerb.SystemDictionary, 1, 1, 1);
                    break;

                default:
                    i.DefaultVerb = DefaultVerb.DontKnow;
                    break;
            }

            return i;
        }
    }
}

