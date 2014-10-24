using System;

namespace lo_novo.PommeDeTerre
{
    public class SalesmanControlRoomThing : Thing
    {
        public bool FlashbackEnabled = true;
        public bool FlashbackDone = false;

        public SalesmanControlRoomThing()
        {
            Name = "salesman";
            AddAliases("(trusty )?rusty", "rusty's( trusty)?( slightly[ \\-]used)?( space ?mobile)?( emporium)?");
        }

        public bool DoSalesmanControlRoomThingThing()
        {
            if (!FlashbackEnabled)
                State.o("You have no time to consider the SALESMAN and his past antics at this point - more pressing matters are afoot.");
            else if (FlashbackDone)
                State.o("With the subtly placed and well-written flashback over, you wish only to purge the blasted SALESMAN from your mind.");
            else
            {
                State.o("At " + State.Player.Name + "'s behest, you find yourselves dragged into a flashback. It was many moons ago...");
                State.TravelAll(typeof(SalesmanFlashbackRoom));
                FlashbackDone = true;
            }

            return true;
        }
    }

    public class SalesmanFlashbackRoom : Room
    {
        public SalesmanFlashbackRoom()
        {
            Name = "TRUSTY RUSTY'S SLIGHTLY-USED SPACEMOBILE EMPORIUM";
            Description = @"Regarding the EMPORIUM, it's some big-ass SPACE SHOWROOM with SHINY SHIPS and LESS SHINY SHIPS dotted around.
PROTO-MARBLE is what the floor is, the ceiling's too far up to make out against the SPOTLIGHTS on the SHINY SHIPS, and the air is stale like last year's Toblerone.
So many SALESMEN, yet you're the only ones there. At risk of belabouring the point, they're sharks, and you're meat.";
        }

        public bool first = true;

        public override void Enter()
        {
            if (first)
            {
                State.o(@"You land in grainy black 'n' white, in cycles gone by, in the past.
Beady-eyed, sweaty men with little hair glance at you, but the eye-fucking by RUSTY hisself shows you that you're exactly one man's meal today.");
                first = false;
            }
                

            base.Enter();
        }

    }

    public class SalesmanConversation : Conversation
    {
        public SalesmanConversation(Room room) : base(room) { }

        public override void Start()
        {
            base.Start();

            State.o("The SALESMAN licks his lip and starts his spiel. Hoo boy.");
            State.o("<SALESMAN> Ah, " + State.AllPlayers.ChooseRandom().Name + ", just the space person I've been waiting for!");

            AddChoice("How do you know who we are?", () => {

                return true;
            });
            AddChoice("Rusty, I presume?", () => {
                State.o("<SALESMAN> By name, but not by nature, hahahahaha!!!");
                State.o("You fail to see the joke.");
                State.o(@"<SALESMAN> Listen, I've been selling ships to young go-getters such as yourselves for many moons, and I think I know exactly what you need.
<SALESMAN> Tell me, have you seen the SUPERROT9001?");

                AddChoice(@"... What about it?", null);
                //AddChoice(@"
                return true;
            });
            AddChoice("We'd rather look around ourselves.", () => {

                return true;
            });

            FinishChoices();
        }
    }
}

