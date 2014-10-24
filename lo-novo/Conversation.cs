using System;
using System.Collections.Generic;
using System.Linq;

namespace lo_novo
{
    public abstract class Conversation
    {
        // choices go A1..n, B1..n, C1..n, etc., so choice spam from players doesn't impact next choice
        internal char choicePrefix = 'A';
        internal int choiceIdx = 1;

        // last set of things we added to the room's custom regex list
        internal List<Tuple<string, EatsNoun, Func<Intention, bool>>> prevRegex
            = new List<Tuple<string, EatsNoun, Func<Intention, bool>>>();
        internal List<string> prevChoicePrompts = new List<string>();

        internal List<Tuple<string, EatsNoun, Func<Intention, bool>>> regex
            = new List<Tuple<string, EatsNoun, Func<Intention, bool>>>();
        internal List<string> choicePrompts = new List<string>();

        internal Room room;

        public Conversation(Room room)
        {
            this.room = room;
        }

        /// <summary>
        /// Adds the choice.
        /// </summary>
        /// <param name="line">what the player can choose to say</param>
        /// <param name="fun">to be executed if the player chooses this. return true if new set of choices</param>
        public void AddChoice(string line, Func<bool> fun)
        {
            regex.Add(new Tuple<string, EatsNoun, Func<Intention, bool>>(
                ("^" + choicePrefix + choiceIdx.ToString() + "$").ToLowerInvariant(),
                EatsNoun.Zero,
                ((intent) => {
                    var b = fun();
                    if (!b)
                        State.o("(Conversation choices are the same as before. Type 'choices' for reminder.)");
                    return true;
                })
            ));
            choicePrompts.Add(choicePrefix + choiceIdx.ToString() + ": " + line);
            choiceIdx++;

        }

        protected virtual void addReminderRegex()
        {
            regex.Add(new Tuple<string, EatsNoun, Func<Intention, bool>>(
                "^choices$",
                EatsNoun.Zero,
                ((intent) => {
                    foreach (var pcp in prevChoicePrompts)
                        State.o(pcp);
                    return true;
                })
            ));
        }

        /// <summary>
        /// Indicates the choices & regex are finalized, letting the player take another action.
        /// </summary>
        public void FinishChoices()
        {
            choicePrefix++;
            if (choicePrefix > 'Z')
                choicePrefix = 'A';

            choiceIdx = 1;

            foreach (var cp in choicePrompts)
                State.o(cp);

            prevChoicePrompts = choicePrompts;

            addReminderRegex();

            room.CustomRegex.RemoveAll((t) => prevRegex.Contains(t));
            room.CustomRegex.AddRange(regex);

            prevRegex = regex;
            regex.Clear();

            choicePrompts.Clear();
        }

        public virtual void Start()
        {
            choicePrefix = 'A';
            regex.Clear();
            choicePrompts.Clear();
        }

        public virtual void Stop()
        {
            room.CustomRegex.RemoveAll((t) => prevRegex.Contains(t));
            prevRegex.Clear();
            prevChoicePrompts.Clear();
            regex.Clear();
            choicePrompts.Clear();
        }
    }
}

