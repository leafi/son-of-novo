using System;

namespace lo_novo
{
    // Produces (mostly) short flavour sentences.
    // We'll have to take some liberties with the writing because it's not easy
    //  to compose e.g. 'James, you'll never pull this one off. I'd rather use a dog.'
    // Instead, text will be like (adlib) 'James, you're incredibly ugly.' + 
    //                            (script) 'I think I'm going to trust this task to someone else.'
    // Ease of writing >> good writing
    public static class Adlib
    {
        private static string GetName(INoun subject)
        {
            if (subject is Player)
                return (subject as Player).Name;
            else if (subject is NPC)
                return (subject as NPC).Name;
            else if (subject is Thing)
                return (subject as Thing).Name;
            else
                return null;
        }

        public static string Refuses(INoun subject = null)
        {
            if (subject is Player)
                return new string[] { (subject as Player).Name + " politely refuses your request.",
                    (subject as Player).Name + " steadfastly refuses.",
                    (subject as Player).Name + " suggests better results may be obtained by removing your head from an orifice." }
                    .ChooseRandom();
            else
                throw new NotImplementedException();
        }

        public static string AttackFace(INoun subject = null)
        {
            var possess = (subject is NPC || subject is Player) ? "their" : "its";
            var name = GetName(subject);

            throw new NotImplementedException();
        }

        public static string Compliment(INoun subject = null)
        {
            // e.g. Tom, you're a smart one.
            var name = GetName(subject);
            var youre = "you're";  // 'its'?
            var you = "you"; // 'it'?

            if (name != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                return new string[] {
                    youre + " smart and surprisingly attractive.",
                    youre + " like the cross between an albatross and a coffee table, a goddamn mess but I still love " + you + ".",
                    youre + " unbelievable. In a good way.",
                    youre + " not the first to cross my path, but you sure are the best.",
                    youre + " the best. A-rou-nd.",
                    youre + " a brilliant concept, all sausage meat and no substitute sausage meat.",
                    youre + " a machine.",
                    youre + " something to be envied, fer sure.",
                    youre + " like a candle in the wind on a still day.",
                    youre + " unstoppable!",
                    youre + " unkeepdownabable."
                }.ChooseRandom().FirstCaps();
            }
        }

        public static string Insult(INoun subject = null)
        {
            // e.g. James, you're a dithering idiot.
            // or That orange is worthless.
            // If null, just use 'you'.
            throw new NotImplementedException();
        }

        public static string Threaten(INoun subject = null)
        {
            // e.g. I'm going to kill you.
            // ENEMIES WHO DON'T KNOW YOU SHOULD KEEP SUBJECT SET TO NULL WHEN CALLING THIS.
            // (They shouldn't know your name or much of anything about you.)
            throw new NotImplementedException();
        }

        public static string Tic(INoun noun = null)
        {
            // e.g. SomeGuy sniffles a bit and rubs his nose.
            // If null, probably use 'It'.
            throw new NotImplementedException();
        }
    }
}

