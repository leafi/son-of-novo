using System;

namespace lo_novo.PommeDeTerre
{
    public class BedRoom : Room
    {
        public BedRoom()
        {
            Name = "BEDR O'ER SUN";

            Description = @"A dusty glaze coats your room, and the ancient red sun stares you dead-on. 
Safe but for the viewshield, you feel the warm light falling onto your skin. Tiny particles, visible for microls, walk between the strong rays coming from Big Ember.
Your room is a mess, but the ancient and terrible dance of the universe continues.";

            Contents.Add(new Thing("mess",
                description: "The mess is sprawling. You casually wonder how long it'll take before"
                + " it realises its own power and moves to take over the entire universe and everything in it."
                + "\nFrankly, it looks like it's about halfway there.",
                activate: "It's inert, thank the gods.",
                attack: "No effect. It's simply too strong.",
                pushPull: @"The problem with that is that there is no obvious place where the mess begins and where it ends.
You apply pressure to a close area, but it simply resumes its original shapeless shape once you stop.",
                talk: "No response. If alive, it is incapable of speech yet - or is lulling you into a false sense of security.",
                take: "You move to do so, but suddenly halt. Was that a growl?",
                punt: "Where would you even start?",
                stop: "No. It's gone too far already. Best to just let it happen.",
                climbDescend: @"No point scaling the thing without proper preparation.
You'd need a week's rations, proper climbing equipment and a guide native to the area, at the very least."
            ));
            Find("mess").AliasesRegex.AddRange(new string[] { "messy room", "sprawling mess", "room mess" });

            Contents.Add(new Thing("dusty glaze coat", "Something you made up.",
                take: "Ha-ha. It is to laugh. The coat (correctly) rejects your attempt as facetious."));
            Find("dusty glaze coat").AddAliases("coat", "dusty coat", "glazed coat");

            Contents.Add(new Thing("light",
                description: "Floating through the viewshield, it's warm and comforting. A real ray of sunshine.",
                activate: "It is active on many parts of the spectrum, without your help.",
                attack: "It passes through you, or the other way round.",
                pushPull: "It passes through you, or, perhaps, the other way around.",
                talk: "It says warmth, and colour, and life, and that it'll be here long after you're gone, though not *here* here.",
                take: "You put some in your pocket. But checking just a single second later - it's gone! Where did it go?!",
                punt: "The sun throws light on you, not the other way around.",
                stop: "No, it's yellow.",
                climbDescend: "Particles of dust appear to be climbing the rays. You'll have a harder time."));
            Find("light").AddAliases("warm light", "(red )?sun ?light", "sunshine");



        }
    }
}
