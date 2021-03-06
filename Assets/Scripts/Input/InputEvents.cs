using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexagon.UserInput
{
    [CreateAssetMenu(menuName = "Hexagon/Input/InputEvents")]
    public class InputEvents : ScriptableObject
    {
        [SerializeField]
        private InputData _inputData;

        private Vector3 _positionOnFirstTouch;
    
        public delegate void InputEvent(bool isClockwise);

        public static event InputEvent OnSwipe; 

        public void ProcessInput(Camera camera)
        {
            GetPositionOnFirstTouch(camera);
            HandleRightSwipe(camera);
            HandleLeftSwipe(camera);
        }

        private void GetPositionOnFirstTouch(Camera camera)
        {
            if (_inputData.GetTouchDown())
            {
                _positionOnFirstTouch = camera.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        private void HandleRightSwipe(Camera camera)
        {
            if (_inputData.GetTouch())
            {
                if ((_positionOnFirstTouch.x - camera.ScreenToWorldPoint(Input.mousePosition).x) < (-_inputData.SwipeDistance))
                {
                    OnSwipe?.Invoke(true);
                }
            }
        }
        
        private void HandleLeftSwipe(Camera camera)
        {
            if (_inputData.GetTouch())
            {
                if ((_positionOnFirstTouch.x - camera.ScreenToWorldPoint(Input.mousePosition).x) > _inputData.SwipeDistance)
                {
                    OnSwipe?.Invoke(false);
                }
            }
        }
    }
}
