using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace lo_novo
{
    // Don't forget you also have .Tags on the Room as a possibly better option!
    // TODO: (Should .Tags reading be merged into TextMine w/Tags having higher scoring words?)
    public static class TextMine
    {
        static Dictionary<string, string> NameToFilename = new Dictionary<string, string>();
        static Dictionary<string, string> NameToCode = new Dictionary<string, string>();

        private static void mineFile(string file)
        {
            if (file.EndsWith(".cs"))
            {
                var s = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                s = s.Substring(0, s.Length - 3);
                NameToFilename.Add(s, file);
                Console.WriteLine(s + ": " + file);
                NameToCode.Add(s, File.ReadAllText(file));
            }
        }

        private static void mineDir(string dir)
        {
            if (dir.Contains("bin") || dir.Contains("obj") || dir.Contains("ircdotnet-"))
                return;

            foreach (var f in Directory.GetFiles(dir))
                mineFile(f);

            foreach (var d in Directory.GetDirectories(dir))
                mineDir(d);
        }

        public static void Setup()
        {
            Debug.Assert(File.Exists("lo-novo.csproj"));
            mineDir(".");
        }

        public static string GetCode(object ting)
        {
            return NameToCode[ting.GetType().Name];
        }

        public static string Best(string[] ss)
        {

            //foreach (string s in ss)
            throw new NotImplementedException();
        }
    }
}

