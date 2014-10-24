using System;

namespace lo_novo
{
    public class DefaultRoomResponses : IHandleDispatch
    {
        protected Room room;

        public DefaultRoomResponses(Room r)
        {
            room = r;
        }

        public virtual bool Dispatch(Intention i)
        {
            // Try dispatch to ActiveNoun first.
            if (i.ActiveNoun != null && (i.ActiveNoun is IHandleDispatch) && (i.ActiveNoun as IHandleDispatch).Dispatch(i))
                return true;

            if (i.DefaultVerb == DefaultVerb.Look)
                return look(i);

            return false;
        }

        private bool look(Intention i)
        {
            if (i.WholeRoom)
            {
                State.o(room.GetFullDescription());
                if (i.PassiveNoun != null)
                    State.o("You didn't find " + i.PassiveNoun.ToString() + " particularly helpful.");
                return true;
            }
            else if (i.ActiveNoun is Thing && room.Contents.Contains(i.ActiveNoun as Thing))
            {
                State.o((i.ActiveNoun as Thing).Description);
                return true;
            }
            else if (i.ActiveNoun is Player)
            {
                State.o((i.ActiveNoun as Player).Name + " sits there, inert. What the hell are they doing?");
                return true;
            }

            return false;
        }
    }
}

