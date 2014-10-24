using System;
using System.Collections.Generic;
using System.Linq;

namespace lo_novo.PommeDeTerre
{
    public class ControlRoom : Room
    {
        public enum StateEnum
        {
            AInit,
            BConversation,
            CExplore
        }

        public StateEnum RoomState = StateEnum.AInit;

        public void Transition(StateEnum next)
        {
            if ((RoomState == StateEnum.AInit && next != StateEnum.BConversation)
                || (RoomState == StateEnum.BConversation && next != StateEnum.CExplore)
                || (RoomState == StateEnum.CExplore))
                throw new Exception(string.Format("incorrect room state transition. was {0} requested {1}", RoomState, next));

            RoomState = next;


        }

        public ControlRoom()
        {
            Name = "POMME DE TERRE: CONTROL ROOM";
            Description = @"You see a dark fog, punched through by the DIM RED HUE of the EMERGENCY LIGHTS.
Eventually, your eyes adjust, and you're able to perceive the room and its contents.
You're in the POMME DE TERRE - the APPLE OF THE EARTH, the ship of you and your COMPADRES, filled with the latest technology of about 50 cycles ago.
Well, the salesman was right about one thing - it was cheap.";
        }
    }
}

