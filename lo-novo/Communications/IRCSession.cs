using System;
using System.Collections.Generic;
using System.Threading;

using IrcDotNet;

namespace lo_novo
{
    public class IRCSession : ITick
    {
        IrcClient client;
        IrcChannel channel;
        public bool Ready = false;
        public Dictionary<IrcUser, PlayerIRC> Players = new Dictionary<IrcUser, PlayerIRC>();
        public List<Player> ToAdd = new List<Player>();
        public List<Player> ToRemove = new List<Player>();
        public List<Player> PlayersInLimbo = new List<Player>();

        IRCSystemIRC systemIIRC = null;

        public IRCSystemIRC SystemIRC { get { return systemIIRC; } }

        public class IRCSystemIRC : IIRC
        {
            IRCSession sesh;

            public IRCSystemIRC(IRCSession session) { this.sesh = session; }

            public string TryRead() { throw new NotImplementedException(); }

            public void Send(string s, bool whisper = false)
            {
                foreach (var line in s.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    sesh.client.LocalUser.SendMessage(sesh.channel, line);
            }
        }

        public class PlayerIRC : IIRC
        {
            IRCSession sesh;
            IrcUser target;
            public Queue<string> Inbox = new Queue<string>();
            public Player Player;

            public PlayerIRC(IRCSession session, IrcUser target) {
                this.sesh = session;
                this.target = target;
            }

            public string TryRead()
            {
                lock (Inbox)
                    if (Inbox.Count > 0)
                        return Inbox.Dequeue();

                return null;
            }

            public void Send(string s, bool whisper = false)
            {
                if (!whisper)
                    foreach (var line in s.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        sesh.client.LocalUser.SendMessage(sesh.channel, line);
                else
                    foreach (var line in s.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        sesh.client.LocalUser.SendMessage(target, line);
            }
        }

        public IRCSession()
        {
            client = new IrcClient();
            client.Disconnected += (object sender, EventArgs e) => {
                Console.WriteLine("DISCONNECTED. (" + e.ToString() + ")");
            };
            client.ConnectFailed += (object sender, IrcErrorEventArgs e) => {
                Console.WriteLine("CONNECT FAILED. (" + e.ToString() + ")");
            };
            client.Connected += (object sender, EventArgs e) => {
                Console.WriteLine("CONNECTED. (" + e.ToString() + ")");
            };
            client.Registered += (object sender, EventArgs ev) => {
                Console.WriteLine("REGISTERED. (" + ev.ToString() + ")");
                client.LocalUser.JoinedChannel += (object ss, IrcChannelEventArgs e) => {
                    if (e.Channel.Name.ToLower().Contains("#lo-novo"))
                    {
                        Console.WriteLine("Joined, I think!");

                        channel = client.Channels[0];
                        systemIIRC = new IRCSystemIRC(this);

                        channel.UserLeft += (object sss, IrcChannelUserEventArgs eleft) => {
                            if (Players.ContainsKey(eleft.ChannelUser.User))
                            {
                                lock (PlayersInLimbo)
                                    PlayersInLimbo.Add(Players[eleft.ChannelUser.User].Player);
                                lock (ToRemove)
                                    ToRemove.Add(Players[eleft.ChannelUser.User].Player);
                                State.AllIRC.Send(Players[eleft.ChannelUser.User].Player.Name + " placed in limbo.");
                            }
                        };
                        channel.MessageReceived += (object esender, IrcMessageEventArgs e2) => {
                            if (e2.Source is IrcUser)
                            {
                                var u = e2.Source as IrcUser;
                                var t = e2.Text;

                                if (t.StartsWith(">") && channel.Users.Contains(channel.GetChannelUser(u)))
                                {
                                    if (Players.ContainsKey(u))
                                    {
                                        lock (Players[u].Inbox)
                                            Players[u].Inbox.Enqueue(t.Substring(1));
                                    }
                                    else
                                    {
                                        lock (PlayersInLimbo)
                                            foreach (var pil in new List<Player>(PlayersInLimbo))
                                                if (pil.Name.ToLower() == u.NickName)
                                                {
                                                    pil.IRC = new PlayerIRC(this, u);
                                                    (pil.IRC as PlayerIRC).Player = pil;
                                                    lock (ToAdd)
                                                        ToAdd.Add(pil);
                                                    PlayersInLimbo.Remove(pil);
                                                    Players.Add(u, pil.IRC as PlayerIRC);
                                                    State.AllIRC.Send(pil.Name + ": Resuming existing character.");
                                                    return;
                                                }

                                        var p = new Player();
                                        p.Name = u.NickName;
                                        p.IRC = new PlayerIRC(this, u);
                                        (p.IRC as PlayerIRC).Player = p;
                                        p.DefaultInventoryResponses = new DefaultInventoryResponses(p);
                                        lock (ToAdd)
                                            ToAdd.Add(p);
                                        State.AllIRC.Send("Welcome to the game, " + p.Name + ".");
                                        Players.Add(u, p.IRC as PlayerIRC);
                                    }
                                }
                            }
                            else
                                Console.WriteLine("message from non-channel user " + e2.Source.ToString());
                        };
                        Ready = true;
                    }
                };

                client.Channels.Join(new string[] { "#lo-novo" });
 
            };
            var ircReg = new IrcUserRegistrationInfo();
            ircReg.NickName = "lo-novo";
            ircReg.RealName = "lo-novo game bot";
            ircReg.UserName = "lo-novo";
            client.FloodPreventer = new IrcStandardFloodPreventer(3, 2000);
            client.Connect("orwell.freenode.net", false, ircReg);
            //client.Channels.
            //client.LocalUser.SendMessage(client.Channels[0].
            //
        }

        public void Tick()
        {
            lock (ToAdd)
                foreach (var a in new List<Player>(ToAdd))
                {
                    State.AllPlayers.Add(a);
                    State.RebuildNameToPlayer();

                    if (a.Room == null)
                    {
                        State.Player = a;
                        State.Travel(typeof(Lobby));
                    }

                    ToAdd.Remove(a);
                }

            lock (ToRemove)
                foreach (var r in new List<Player>(ToRemove))
                {
                    State.AllPlayers.Remove(r);
                    State.RebuildNameToPlayer();
                    ToRemove.Remove(r);
                }
        }

    }
}

