using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexagon.State
{
    public class StateManager
    {
        public enum State
        {
            EMPTY,
            WAITING,
            ROTATING,
            DESTROYING,
            FILLING,
            CHECKING,
            GAME_OVER
        }

        public static State CurrentState = State.CHECKING;
    }
}
