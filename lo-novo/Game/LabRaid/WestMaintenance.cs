using System;
using System.Collections.Generic;
using System.Linq;

namespace lo_novo.LabRaid
{
    public class WestMaintenance : Room
    {
        public class Wire : Thing
        {
            public bool Cut = false;

            public Wire(string colour) : base(colour + " wire")
            {
            }
        }

        public class Pliers : Thing
        {
            public Pliers()
            {
                Name = "pliers";
            }
        }

        public Thing OxygenControls;

        private bool twistOxygenDial(Intention i)
        {
            if (LabRaidState.Oxygen > 0) // gets stuck OFF
            {
                // Oxygen % ALWAYS lowers.
                // Need some custom actions/custom Thing code so if they try and turn it back we acknowledge this...??? Yeah! "too late..."

                State.o("You twist the crusty dial, and the oxygen level lowers.");

                if (LabRaidState.Oxygen == 100)
                {
                    //State.Ticking.Add(new SimpleTick(() => {
                    // BEGIN DEATH TIMER.

                    //}));
                }

                LabRaidState.Oxygen -= (State.RNG.Next() > 0.5 ? 20 : 30);
                LabRaidState.Oxygen = (LabRaidState.Oxygen < 0 ? 0 : LabRaidState.Oxygen);

                if (LabRaidState.Oxygen < 20)
                { /* .. */
                }
                else if (LabRaidState.Oxygen < 50)
                { /* .. */
                }
                else if (LabRaidState.Oxygen < 81)
                { /* .. */
                }
            }
            else
                State.o("No good. It's firmly stuck at 0%.\nIs it just me, or is it a bit stuffy in here?");
            // TODO: UPDATE OBJECT DESC BASED ON OXY %. And possibly a bunch of responses when it gets desperate.

            return true;
        }

        public WestMaintenance()
        {   
            Name = "Maintenance Cupboard (W)";

            var t1 = new Thing("low ceiling", "An exceedingly low ceiling, for such a spacious spacious-ship.");
            t1.Set(DefaultVerb.Push, "Raise the roof!");
            t1.Announce = false;
            Contents.Add(t1);

            OxygenControls = new Thing("oxygen supply control", 
                @"This looks like it controls the oxygen supply for the entire ship! It's currently at 100%.
                The dial seems far too tempting.", 
                take: "You'd take away all oxygen on ship?! You monster!", // or "That's just greedy." <- mebbeh too confusing tho
                activate: twistOxygenDial,
                modify: twistOxygenDial,
                pushPull: "It's a turny knob, not a pushy-pulley.",
                talk: "I talk to the wind. But the wind cannot hear...",
                punt: "That's a worrying noise..." // hahaha!
            );
            Contents.Add(OxygenControls);

            // NOT added unless party fucks up the cables
            var fireAdded = false;
            var fire = new Thing("small but growing fire",
                           @"A worrying fire near the power controls. Is this what they call an electrical fire?
It's spreading slowly towards the safe."
                       );


            var redWire = new Wire("red");
            var blueWire = new Wire("blue");
            var purpleWire = new Wire("purple");
            var mauveWire = new Wire("mauve");
            var burgundyWire = new Wire("burgundy");
            Contents.AddRange(new Wire[] { redWire, blueWire, purpleWire, mauveWire, burgundyWire });

            var cables = new Thing("electricity cables",
                             @"Wires, frayed and rusted, run from the ceiling to the floor. Red, blue, purple and mauve.
What's a humble space explorer to do?",
                             take: @"Yeah, of course, take the LIVE WIRES. That'll end well.
The only thing you're taking, sir, is the piss!",
                             punt: Func((i) => { // arg passed in will be Intention; the parsing result object
                    if (State.RNG.Next() > 0.2)
                    {
                        // spark produced -- if high oxygen, this creates a fireball
                        //State.Player.Eyebrows = false;
                    }
                    else
                    {
                        // fire produced if oxygen available
                        // fire might actually be useful! k.
                        if (LabRaidState.Oxygen > 20 && !fireAdded)
                        if (i.Violence > 3)
                        {
                            State.o("You violently " + i.VerbString + " the cabling, and much sparking occurs.");
                            
                        }
                        else
                            State.o("You pathetically " + i.VerbString + " the cables. They rattle, but they aren't. Uh, rattled.");
                        else if (fireAdded)
                            State.o("Another electrical fire completely fails to occur. Thank Sol the oxygen is so low!");
                        else
                            State.o("Some embers break over the wall, but the lack of oxygen means nothing ignites.");
                    }

                    return true;
                }),
                             activate: Func((i) => { // for if they're trying to reroute the wires to achieve something?
            
                    // XXX
                    // XXX: TODO: BREAK THIS OUT INTO ROOM SCRIPT. At this point, ActiveNoun must be the cables as a *whole*.
                    // XXX

                    //if (i.ActiveNoun == 
                
                    if ((i.ActiveNoun as Wire).Cut)
                    {
                        State.o("Alas, this wire is already cut."); // oh, good catch.
                        return true;
                    }
                    State.o("This is bound to end well! What a clever idea!");
                    State.o("RED WIRE or BLUE WIRE?");

                    if (i.PassiveNoun is Pliers)
                    {
                        State.o("Smart thinking. That'll keep the electricity out of your insides."); // haahahahaahah
                        if (i.ActiveNoun == redWire)
                        {
                            State.o(@"You honestly cut the RED WIRE? Wow.
You hear the sound of 53 doors and one viewing window all locking in unison."); // hahahahaahahahaha
                            // TODO: add convenience func, deal with case where only 1 player
                            State.o("<" + State.AllPlayers.Except(new Player[] { State.Player }).ChooseRandom().Name + "> " + State.Player.Name + ", you're a fucking moron.");
                            //State.Player.Personality.Oblivious++;
                            LabRaidState.DoorsLocked = true; // every door must now be broken to open?
                            // ^ to support this, should break Exits code out of parser & hack up Exits code. doable though.
                            // should automatically inject un-Important-when-unlocked Door objects into rooms, too...
                        }
                        else if (i.ActiveNoun == blueWire)
                        {
                            State.o("Well, that was a fairly obvious choice.");
                            LabRaidState.SecurityActive = true; // maybe?
                        }
                    }
                    else if (i.PassiveNoun is Player)
                    {
                        if (i.ActiveNoun == redWire)
                        if (LabRaidState.SecurityActive)
                        {
                            State.o((i.PassiveNoun as Player).Name + " agrees, and disconnects the RED WIRE with ease. That went really well!");
                            LabRaidState.SecurityActive = false;
                        }
                        else
                            State.o("They smugly decline to give you a repeat performance.");
                        else
                            Adlib.Refuses(i.PassiveNoun);
                    }
                    else // TODO: perhaps use other tools than just pliers
                    {
                        State.o("YOUCH! Your fingers are all burned and crispy now. And the RED WIRE isn't affected at all.");
                    }

                    // maybe a clue you can find somewhere else? or a puzzle?
                    State.o("MAUVE WIRE or PURPLE WIRE?");
                    if (i.PassiveNoun is Pliers)
                    {

                        State.o("Pliers! Like fingers, but not conductive across the user's chest cavity.");
                        if (i.ActiveNoun == mauveWire)
                        {
                            State.o(@"Mauve is *so* last season.
What did cutting this do?"); // maybe this chooses between two prizes later in the level? easily achieved!
                            LabRaidState.MauveSyndrome = true;

                        }
                        else if (i.ActiveNoun == purpleWire)
                        {
                            State.o("Purple is *in* this season."); // hah!
                            //State.Player.Personality.FashionSense++; < hahaha.

                            if (!LabRaidState.SecurityActive)
                            {
                                State.o(@"Those lights you extinguished when you cut the other wires? They're blink on..
A series of clanks and gutteral noises demonstrate the system's reinvigoration, vigorously."); // hahaha last sentence is awks but i can't think of proper words? nor can I, really this'll do.
                            }
                            else
                                State.o("Strange. Nothing happened.");
                            LabRaidState.SecurityActive = true; // don't push your luck, player
                        
                        }
                    }
                    else // TODO: perhaps use other tools than just pliers
                    {
                        State.o("YOUCH! Your fingers are all burned and crispy now. And the RED WIRE isn't affected at all.");
                    }

                    return true;

                }),
                             talk: "Like a bird on the wire... better stop talking to the wire." // ha.
                         );
            Contents.Add(cables);


            // when you have code for managing Things, everything looks like a Thing. likewise for lambdas...

            Contents.Add(new Thing("extremely useful looking safe" // but it's LOCKED! OMGZ.
            ));


            Contents.Add(new Thing("humour gas controls" // haha
            ));
            AddExit(Direction.East, typeof(WestCorridorS), "corridor");
        }
    }
}

