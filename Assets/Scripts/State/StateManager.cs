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
            DESTROYING_OBJECTS,
            FILLING,
            CHECKING,
            GAME_OVER,
            DESTROYING_SCENE            
        }

        public static State CurrentState = State.CHECKING;
    }
}
