using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexagon.UserInput
{
    public enum MouseButton
    {
        Left,
        Right,
        Middle
    }

    [CreateAssetMenu(menuName = "Hexagon/Input/InputData")]
    public class InputData : ScriptableObject
    {
        [SerializeField] 
        public float SwipeDistance;

        public bool GetTouchDown()
        {
            return Input.GetMouseButtonDown((int) MouseButton.Left);
        }

        public bool GetTouch()
        {
            return Input.GetMouseButton((int) MouseButton.Left);
        }
    }
}