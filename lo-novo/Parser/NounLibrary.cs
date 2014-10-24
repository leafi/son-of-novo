using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace lo_novo
{
    public static class NounLibrary
    {
        /// <summary>
        /// Builds noun library using current state.
        /// </summary>
        public static IEnumerable<Tuple<string, INoun>> Build()
        {
            var list = new List<Tuple<string, INoun>>();
            Action<string, INoun> tu = (s, n) => list.Add(Tuple.Create(s, n));

            foreach (var c in State.Room.Contents)
            {
                foreach (var a in c.AliasesRegex.Union(new string[] { Regex.Escape(c.Name) }))
                {
                    tu(a, c);
                    tu(Regex.Escape(c.Preposition) + " " + a, c);
                    tu("the " + a, c);
                    tu("a ", c);
                }
            }

            var ownership = new List<string>();

            ownership.AddRange(new string[] { "my ", "one's own ", "one's ", "own ", "bag " });
            foreach (var s in State.Player.Aliases.Union(new string[] { State.Player.Name }))
                ownership.Add(Regex.Escape(s) + "'s ");

            foreach (var c in State.Player.Inventory)
            {
                foreach (var a in c.AliasesRegex.Union(new string[] { Regex.Escape(c.Name) }))
                {
                    tu(a, c);
                    tu(Regex.Escape(c.Preposition) + " " + a, c);
                    tu("the " + a, c);
                    tu("a " + a, c);

                    foreach (var o in ownership)
                        tu(o + a, c);
                }
            }

            foreach (var s in new string[] { "me", "myself", "oneself", "this guy", "this girl",
                "this gal", "this fool", "the best guy", "protagonist", "i", "self" })
                tu(s, State.Player);

            foreach (var p in State.AllPlayers)
            {
                tu(Regex.Escape(p.Name), p);
                foreach (var a in p.Aliases)
                {
                    tu(Regex.Escape(a), p);
                }
            }

            tu(Regex.Escape((State.Room.Name ?? "ENONAME")), State.Room);
            tu("room", State.Room);

            // ugh... fix up regexp :x
            return list.ConvertAll((t) => Tuple.Create("^" + t.Item1.ToLowerInvariant() + "$", t.Item2));
        }
    }
}

