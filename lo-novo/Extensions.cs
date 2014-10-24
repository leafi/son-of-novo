using System;
using System.Collections.Generic;
using System.Linq;

namespace lo_novo
{
    public static class Extensions
    {
        public static T ChooseRandom<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ElementAt(State.RNG.Next(enumerable.Count()));
        }

        public static IEnumerable<string> JoinedAndSplit(this IEnumerable<string> words)
        {
            var max = words.Count();

            for (int i = max; i > 0; i--)
                for (int j = 0; j < max - i + 1; j++)
                    yield return string.Join(" ", words.Skip(j).Take(i));
        }

        public static IEnumerable<string> PermuteWith(this IEnumerable<string> first, IEnumerable<string> second)
        {
            foreach (var f in first)
                foreach (var s in second)
                    yield return f + s;
        }

        public static string FirstCaps(this string s)
        {
            return char.ToUpperInvariant(s[0]) + s.Substring(1);
        }
    }
}

