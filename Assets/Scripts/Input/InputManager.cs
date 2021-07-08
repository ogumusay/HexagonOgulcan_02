using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
            _inputEvents.ProcessInput(_cameraMain);
        }
    }
}
