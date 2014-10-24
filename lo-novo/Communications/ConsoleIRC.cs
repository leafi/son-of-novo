using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lo_novo.Communications
{
    // Dumb, fake IRC that can't whisper. Primarily for debugging. Thread-safe.
    public class ConsoleIRC : ITick
    {
        public class SystemIIRC : IIRC
        {
            internal SystemIIRC() { }

            public string TryRead() { throw new NotSupportedException(); }

            public void Send(string s, bool whisper = false)
            {
                Console.WriteLine(s);
            }
        }

        public class PlayerIIRC : IIRC
        {
            private ConsoleIRC circ;
            internal PlayerIIRC(ConsoleIRC circ) { this.circ = circ; }

            public string TryRead()
            {
                lock (circ.input)
                    return circ.input.Count > 0 ? circ.input.Dequeue() : null;
            }

            public void Send(string s, bool whisper = false)
            {
                Console.ForegroundColor = whisper ? ConsoleColor.Gray : ConsoleColor.DarkGreen;
                Console.WriteLine(s);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        SystemIIRC systemIIRC;
        PlayerIIRC playerIIRC;

        public IIRC SystemIRC { get { return systemIIRC; } }
        public IIRC PlayerIRC { get { return playerIIRC; } }

        protected Queue<string> input = new Queue<string>();

        public ConsoleIRC()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            systemIIRC = new SystemIIRC();
            playerIIRC = new PlayerIIRC(this);

            // listen for input on separate thread
            new Thread(() =>
            {
                while (true)
                {
                    var s = Console.ReadLine();
                    lock (input)
                        input.Enqueue(s);
                }
            }).Start();
        }

        public void Tick() { }

    }
}
