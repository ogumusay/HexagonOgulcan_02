using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexagon.State;


namespace Hexagon.UserInput
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        private InputEvents _inputEvents; 

        private Camera _cameraMain;

        private void Start() 
        {
            _cameraMain = Camera.main;
        }

        private void Update() 
        {
            if (StateManager.CurrentState == StateManager.State.EMPTY)
            {
                _inputEvents.ProcessInput(_cameraMain);                
            }
        }
    }
}
