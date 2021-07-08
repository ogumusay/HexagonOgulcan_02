using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexagon.State
{
    public class StateManager : ScriptableObject
    {
        public enum State
        {
            EMPTY,
            ROTATING,
            DESTROYING,
            FILLING,
            CHECKING
        }

        public static State CurrentState;
    }
}
